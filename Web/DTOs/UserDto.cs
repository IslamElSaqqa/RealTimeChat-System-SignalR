using Newtonsoft.Json;

namespace Advanced.Web.DTOs
{
    public class UserDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}
