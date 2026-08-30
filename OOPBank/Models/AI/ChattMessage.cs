namespace BankSystem.Models.AI;

public class ChattMessage
{
    public int Id { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;


    // Foreign Key
    public int ConversationId { get; set; }


    // Navigation Property
    public Conversation? Conversation { get; set; }
}