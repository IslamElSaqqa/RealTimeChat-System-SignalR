namespace API.DTOs
{
    public class CreateRoomDTO
    {
        // Both Web and Desktop clients POST { "roomName": "..." } as the JSON body.
        // System.Text.Json's default model binder matches property names
        // case-insensitively, so "roomName" in the JSON maps to "RoomName" here.
        public string RoomName { get; set; } = string.Empty;
    }
}
