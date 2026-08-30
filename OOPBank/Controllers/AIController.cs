using BankSystem.Data;
using BankSystem.Models.AI;
using BankSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BankSystem.Controllers;

public class AIController : Controller
{
    private readonly AIService _aiService;
    private readonly AppDbContext _context;
    private readonly KnowledgeService _knowledgeService;

    public AIController(
        AIService aiService,
        AppDbContext context, KnowledgeService knowledgeService)
    {
        _aiService = aiService;
        _context = context;
        _knowledgeService = knowledgeService;
    }

    [HttpGet]
    public async Task<IActionResult> Chat()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(
                "Index",
                "Account"
            );
        }

        var conversation = await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (conversation == null)
        {
            conversation = new Conversation
            {
                UserId = userId
            };

            _context.Conversations.Add(conversation);

            await _context.SaveChangesAsync();
        }

        var viewModel = new ChatViewModel
        {
            Messages = conversation.Messages
                .OrderBy(m => m.CreatedAt)
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Chat(ChatViewModel model)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(
                "Index",
                "Account"
            );
        }

        var conversation = await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(
                c => c.UserId == userId
            );

        if (conversation == null)
        {
            conversation = new Conversation
            {
                UserId = userId
            };

            _context.Conversations.Add(conversation);

            await _context.SaveChangesAsync();
        }

        if (!string.IsNullOrWhiteSpace(model.Message))
        {
            var userMessage = new ChattMessage
            {
                Role = "user",
                Content = model.Message,
                ConversationId = conversation.Id
            };

            _context.ChatMessages.Add(userMessage);

            await _context.SaveChangesAsync();

            try
            {
                var messages = await _context.ChatMessages
               .Where(m => m.ConversationId == conversation.Id)
               .OrderBy(m => m.CreatedAt)
               .ToListAsync();

                var relevantChunks =
                await _knowledgeService.SearchAsync(model.Message)
                ?? new List<KnowledgeChunk>();

                Console.WriteLine("");
                Console.WriteLine("========== RAG DEBUG ==========");
                Console.WriteLine($"QUESTION: {model.Message}");
                Console.WriteLine($"CHUNKS FOUND: {relevantChunks.Count}");

                foreach (var chunk in relevantChunks)
                {
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine(
                        $"DOCUMENT: {chunk.KnowledgeDocument?.Title}"
                    );
                    Console.WriteLine(
                        $"CHUNK INDEX: {chunk.ChunkIndex}"
                    );
                    Console.WriteLine("CONTENT:");
                    Console.WriteLine(chunk.Content);
                }

                Console.WriteLine("========== END RAG DEBUG ==========");
                Console.WriteLine("");

                var answer = await _aiService.AskAIAsync(
                messages,
                relevantChunks
                );

                if (string.IsNullOrWhiteSpace(answer))
                {
                    answer = "Sorry, I could not generate an answer.";
                }

                var aiMessage = new ChattMessage
                {
                    Role = "assistant",
                    Content = answer,
                    ConversationId = conversation.Id
                };

               

                _context.ChatMessages.Add(aiMessage);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                var errorMessage = new ChattMessage
                {
                    Role = "assistant",
                    Content =
                        "The AI assistant is currently unavailable.",
                    ConversationId = conversation.Id
                };

                _context.ChatMessages.Add(errorMessage);

                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction(nameof(Chat));
    }
}