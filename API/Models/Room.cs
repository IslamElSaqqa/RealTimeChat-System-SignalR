namespace API.Models
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string CreatedById { get; set; } = string.Empty;

        public ApplicationUser CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();
    }
}
