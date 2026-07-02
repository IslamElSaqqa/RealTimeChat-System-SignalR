using System.Text.Json;

namespace Desktop.Helpers
{
    public static class TokenStorage
    {
        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdvancedChat");

        private static readonly string TokenFilePath = Path.Combine(AppDataPath, "auth_token.json");
        private static readonly string UserFilePath = Path.Combine(AppDataPath, "user_info.json");

        static TokenStorage()
        {
            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }
        }

        public static void SaveToken(string token)
        {
            try
            {
                var data = new { Token = token, SavedAt = DateTime.UtcNow };
                var json = JsonSerializer.Serialize(data);
                File.WriteAllText(TokenFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving token: {ex.Message}");
            }
        }

        public static string? GetToken()
        {
            try
            {
                if (!File.Exists(TokenFilePath))
                    return null;

                var json = File.ReadAllText(TokenFilePath);
                var data = JsonSerializer.Deserialize<JsonElement>(json);

                if (data.TryGetProperty("Token", out var tokenElement))
                {
                    return tokenElement.GetString();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading token: {ex.Message}");
                return null;
            }
        }

        public static void SaveUserInfo(string userId, string email, string fullName)
        {
            try
            {
                var data = new { UserId = userId, Email = email, FullName = fullName };
                var json = JsonSerializer.Serialize(data);
                File.WriteAllText(UserFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user info: {ex.Message}");
            }
        }

        public static (string? UserId, string? Email, string? FullName) GetUserInfo()
        {
            try
            {
                if (!File.Exists(UserFilePath))
                    return (null, null, null);

                var json = File.ReadAllText(UserFilePath);
                var data = JsonSerializer.Deserialize<JsonElement>(json);

                var userId = data.TryGetProperty("UserId", out var userIdElement) 
                    ? userIdElement.GetString() 
                    : null;

                var email = data.TryGetProperty("Email", out var emailElement) 
                    ? emailElement.GetString() 
                    : null;

                var fullName = data.TryGetProperty("FullName", out var fullNameElement) 
                    ? fullNameElement.GetString() 
                    : null;

                return (userId, email, fullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading user info: {ex.Message}");
                return (null, null, null);
            }
        }

        public static void ClearAll()
        {
            try
            {
                if (File.Exists(TokenFilePath))
                    File.Delete(TokenFilePath);

                if (File.Exists(UserFilePath))
                    File.Delete(UserFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing storage: {ex.Message}");
            }
        }

        public static bool IsTokenAvailable()
        {
            return !string.IsNullOrEmpty(GetToken());
        }
    }
}
