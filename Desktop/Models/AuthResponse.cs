using Newtonsoft.Json;

namespace Desktop.Models
{
    public class AuthResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("user")]
        public UserDto? User { get; set; }
    }

    public class UserDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("fullName")]
        public string FullName { get; set; } = string.Empty;
    }
}
