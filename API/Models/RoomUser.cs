namespace API.Models
{
    public class RoomUser
    {
        public Guid RoomId { get; set; }

        public Room Room { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
    }
}
