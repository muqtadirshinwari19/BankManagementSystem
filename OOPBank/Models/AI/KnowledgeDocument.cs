namespace BankSystem.Models.AI;

public class KnowledgeDocument
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation Property
    public List<KnowledgeChunk> Chunks { get; set; } = new();
}