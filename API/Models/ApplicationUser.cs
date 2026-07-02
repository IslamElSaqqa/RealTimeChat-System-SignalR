using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RoomUser> RoomUsers { get; set; } = new List<RoomUser>();

        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    }
}
