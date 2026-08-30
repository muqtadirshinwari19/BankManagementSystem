using BankSystem.Models.Identities;

namespace BankSystem.Models.AI;

public class Conversation
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<ChattMessage> Messages { get; set; } = new();
}