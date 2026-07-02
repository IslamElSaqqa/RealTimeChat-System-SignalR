namespace Desktop.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public Guid? RoomId { get; set; }
        public string? ReceiverId { get; set; }
        public int MessageType { get; set; } // 1 = Public, 2 = Private
    }
}
