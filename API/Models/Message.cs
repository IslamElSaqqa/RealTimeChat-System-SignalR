namespace API.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public MessageType MessageType { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; } = string.Empty;

        public ApplicationUser Sender { get; set; } = null!;

        public string? ReceiverId { get; set; }

        public ApplicationUser? Receiver { get; set; }

        public Guid? RoomId { get; set; }

        public Room? Room { get; set; }


    }
}
