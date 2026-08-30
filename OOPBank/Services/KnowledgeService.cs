using BankSystem.Data;
using BankSystem.Models.AI;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services;

public class KnowledgeService
{
    private readonly AppDbContext _context;

    public KnowledgeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<KnowledgeChunk>> SearchAsync(
        string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new List<KnowledgeChunk>();
        }

        var stopWords = new HashSet<string>
        {
            "what",
            "is",
            "are",
            "the",
            "a",
            "an",
            "for",
            "to",
            "of",
            "in",
            "on",
            "and",
            "or",
            "do",
            "does",
            "can",
            "i",
            "my",
            "please",
            "tell",
            "me"
        };

        var cleanedQuestion = Normalize(question);

        var words = cleanedQuestion
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            )
            .Where(word =>
                word.Length > 2 &&
                !stopWords.Contains(word)
            )
            .Distinct()
            .ToList();

        var chunks = await _context.KnowledgeChunks
            .Include(c => c.KnowledgeDocument)
            .ToListAsync();

        var scoredChunks = chunks
            .Select(chunk =>
            {
                var text = Normalize(chunk.Content);

                int score = 0;

                // Score individual words
                foreach (var word in words)
                {
                    if (text.Contains(word))
                    {
                        score += 1;
                    }
                }

                // Strong bonus for consecutive phrases
                for (int i = 0; i < words.Count - 1; i++)
                {
                    var phrase =
                        words[i] + " " + words[i + 1];

                    if (text.Contains(phrase))
                    {
                        score += 10;
                    }
                }

                // Strong bonus when all keywords
                // appear in the same chunk
                if (words.All(word => text.Contains(word)))
                {
                    score += 20;
                }

                // Score matching document title
                var title = Normalize(
                    chunk.KnowledgeDocument?.Title ?? ""
                );

                foreach (var word in words)
                {
                    if (title.Contains(word))
                    {
                        score += 5;
                    }
                }

                return new
                {
                    Chunk = chunk,
                    Score = score
                };
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(3)
            .Select(x => x.Chunk)
            .ToList();

        return scoredChunks;
    }

    private string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        return new string(
            text
                .ToLower()
                .Where(c =>
                    char.IsLetterOrDigit(c) ||
                    char.IsWhiteSpace(c))
                .ToArray()
        );
    }
}