using System.Net.Http.Headers;
using Desktop.Helpers;
using Desktop.Models;
using Newtonsoft.Json;

namespace Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7127";

        public ApiService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        private void SetAuthenticationHeader()
        {
            var token = TokenStorage.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequest { Email = email, Password = password };
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
                    if (result?.Token != null)
                    {
                        TokenStorage.SaveToken(result.Token);
                        if (result.User != null)
                        {
                            TokenStorage.SaveUserInfo(result.User.Id, result.User.Email, result.User.FullName);
                        }
                    }
                    return result;
                }

                return JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            }
            catch (Exception ex)
            {
                return new AuthResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse?> RegisterAsync(string email, string fullName, string password, string confirmPassword)
        {
            try
            {
                var request = new RegisterRequest 
                { 
                    Email = email, 
                    FullName = fullName, 
                    Password = password,
                    ConfirmPassword = confirmPassword
                };
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
                    if (result?.Token != null)
                    {
                        TokenStorage.SaveToken(result.Token);
                        if (result.User != null)
                        {
                            TokenStorage.SaveUserInfo(result.User.Id, result.User.Email, result.User.FullName);
                        }
                    }
                    return result;
                }

                return JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            }
            catch (Exception ex)
            {
                return new AuthResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<List<RoomModel>?> GetRoomsAsync()
        {
            try
            {
                SetAuthenticationHeader();
                var response = await _httpClient.GetAsync("/api/rooms");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<RoomModel>>(content);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting rooms: {ex.Message}");
                return null;
            }
        }

        public async Task<RoomModel?> CreateRoomAsync(string roomName)
        {
            try
            {
                SetAuthenticationHeader();
                var json = JsonConvert.SerializeObject(new { roomName });
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/rooms", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<RoomModel>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating room: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteRoomAsync(Guid roomId)
        {
            try
            {
                SetAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"/api/rooms/{roomId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting room: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MessageModel>?> GetRoomMessagesAsync(Guid roomId)
        {
            try
            {
                SetAuthenticationHeader();
                var response = await _httpClient.GetAsync($"/api/messages/room/{roomId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var messages = JsonConvert.DeserializeObject<dynamic>(content);
                    var result = new List<MessageModel>();

                    if (messages != null)
                    {
                        foreach (var msg in messages)
                        {
                            result.Add(new MessageModel
                            {
                                Id = Guid.NewGuid(),
                                Content = msg["content"]?.ToString() ?? "",
                                Sender = msg["sender"]?.ToString() ?? "",
                                SentAt = DateTime.Parse(msg["sentAt"]?.ToString() ?? DateTime.UtcNow.ToString()),
                                RoomId = roomId,
                                MessageType = 1
                            });
                        }
                    }

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting room messages: {ex.Message}");
                return null;
            }
        }

        public async Task<List<UserModel>?> GetUsersAsync()
        {
            try
            {
                SetAuthenticationHeader();
                var response = await _httpClient.GetAsync("/api/users");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<UserModel>>(content);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return null;
            }
        }
    }
}
