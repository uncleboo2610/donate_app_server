namespace ChatRealtimeDemo.Models
{
    public class ChatMessages
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
    }
}
