namespace BankSystem.Models.AI
{
    public class ChatViewModel
    {
        
            public string Message { get; set; } = string.Empty;

            public List<ChattMessage> Messages { get; set; } = new();
        
    }
}
