using Newtonsoft.Json;

namespace Advanced.Web.DTOs
{
    public class AuthResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("user")]
        public UserDto? User { get; set; }
    }
}
