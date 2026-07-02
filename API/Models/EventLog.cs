namespace API.Models
{
    public class EventLog
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Event { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
