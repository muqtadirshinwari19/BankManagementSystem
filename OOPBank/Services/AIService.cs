using BankSystem.Models.AI;
using System.Text;
using System.Text.Json;

namespace BankSystem.Services;

public class AIService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> AskAIAsync(
        List<ChattMessage> messages,
        List<KnowledgeChunk> knowledgeChunks)
    {
        if (knowledgeChunks == null || !knowledgeChunks.Any())
        {
            return "Sorry, I could not find relevant information " +
                   "in the Bank Knowledge Base.";
        }

        // Get the latest user question
        var userQuestion = messages
            .LastOrDefault(m => m.Role == "user")
            ?.Content;

        if (string.IsNullOrWhiteSpace(userQuestion))
        {
            return "Please enter a question.";
        }

        // Get the relevant information from RAG
        var context = string.Join(
            "\n\n",
            knowledgeChunks.Select(chunk =>
                $"Document: {chunk.KnowledgeDocument?.Title}\n" +
                $"Content:\n{chunk.Content}")
        );

        // Prompt sent to the local AI model
        var prompt = $"""
You are a helpful AI assistant for a Bank Management System.

Answer the user's question using ONLY the information
provided in the BANK KNOWLEDGE BASE.

Rules:
1. Give a clear and natural answer.
2. Keep the answer concise.
3. Do not return the entire document.
4. Use bullet points when appropriate.
5. Do not invent information.
6. If the answer is not found in the knowledge base, say:
   "I could not find this information in the bank knowledge base."

BANK KNOWLEDGE BASE:
{context}

USER QUESTION:
{userQuestion}
""";

        var requestBody = new
        {
            model = "llama3.2:1b",
            prompt = prompt,
            stream = false
        };

        var json = JsonSerializer.Serialize(requestBody);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(
            "http://localhost:11434/api/generate",
            content
        );

        response.EnsureSuccessStatusCode();

        var responseJson =
            await response.Content.ReadAsStringAsync();

        using var document =
            JsonDocument.Parse(responseJson);

        var answer = document.RootElement
            .GetProperty("response")
            .GetString();

        return string.IsNullOrWhiteSpace(answer)
            ? "Sorry, I could not generate an answer."
            : answer.Trim();
    }
}