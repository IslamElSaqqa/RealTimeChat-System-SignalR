namespace API.DTOs
{
    public class SendMessageDTO
    {
        public string Message { get; set; } = string.Empty;

        public Guid RoomId { get; set; }
    }
}
