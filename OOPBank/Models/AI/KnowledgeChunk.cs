namespace BankSystem.Models.AI;

public class KnowledgeChunk
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public int ChunkIndex { get; set; }

    // Foreign Key
    public int KnowledgeDocumentId { get; set; }

    // Navigation Property
    public KnowledgeDocument? KnowledgeDocument { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}