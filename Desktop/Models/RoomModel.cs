namespace Desktop.Models
{
    public class RoomModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
