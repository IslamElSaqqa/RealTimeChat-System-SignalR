using Microsoft.AspNetCore.SignalR.Client;
using Desktop.Helpers;
using Desktop.Models;

namespace Desktop.Services
{
    public class SignalRService
    {
        private HubConnection? _connection;
        private readonly string _hubUrl = "https://localhost:7127/chatHub";

        public event Action<string, string, DateTime>? OnRoomMessageReceived;
        public event Action<string, string, DateTime>? OnPrivateMessageReceived;
        public event Action<List<string>>? OnOnlineUsersUpdated;
        public event Action<Guid, string>? OnRoomCreated;
        public event Action<Guid>? OnRoomDeleted;
        public event Action<string>? OnUserJoined;
        public event Action<string>? OnUserLeft;
        public event Action? OnConnected;
        public event Action? OnDisconnected;

        public async Task ConnectAsync()
        {
            try
            {
                var token = TokenStorage.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("No authentication token available");
                }

                _connection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl, options =>
                    {
                        options.AccessTokenProvider = async () => token;
                        options.HttpMessageHandlerFactory = _ =>
                        {
                            var handler = new HttpClientHandler();
                            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                            return handler;
                        };
                    })
                    .WithAutomaticReconnect()
                    .Build();

                RegisterEventHandlers();

                await _connection.StartAsync();
                OnConnected?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                throw;
            }
        }

        private void RegisterEventHandlers()
        {
            if (_connection == null)
                return;

            _connection.On<string, string, DateTime>("ReceiveRoomMessage", (sender, message, time) =>
            {
                OnRoomMessageReceived?.Invoke(sender, message, time);
            });

            _connection.On<string, string, DateTime>("ReceivePrivateMessage", (sender, message, time) =>
            {
                OnPrivateMessageReceived?.Invoke(sender, message, time);
            });

            _connection.On<List<string>>("OnlineUsers", (users) =>
            {
                OnOnlineUsersUpdated?.Invoke(users);
            });

            _connection.On<Guid, string>("RoomCreated", (roomId, roomName) =>
            {
                OnRoomCreated?.Invoke(roomId, roomName);
            });

            _connection.On<Guid>("RoomDeleted", (roomId) =>
            {
                OnRoomDeleted?.Invoke(roomId);
            });

            _connection.On<string>("UserJoined", (userId) =>
            {
                OnUserJoined?.Invoke(userId);
            });

            _connection.On<string>("UserLeft", (userId) =>
            {
                OnUserLeft?.Invoke(userId);
            });

            _connection.Closed += async (error) =>
            {
                OnDisconnected?.Invoke();
                await Task.Delay(5000);
                try
                {
                    await ConnectAsync();
                }
                catch
                {
                    // Reconnection failed, will be retried by automatic reconnect
                }
            };
        }

        public async Task JoinRoomAsync(Guid roomId)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("JoinRoom", roomId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error joining room: {ex.Message}");
            }
        }

        public async Task LeaveRoomAsync(Guid roomId)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("LeaveRoom", roomId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error leaving room: {ex.Message}");
            }
        }

        public async Task SendRoomMessageAsync(Guid roomId, string message)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("SendRoomMessage", roomId, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending room message: {ex.Message}");
            }
        }

        public async Task SendPrivateMessageAsync(string receiverId, string message)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("SendPrivateMessage", receiverId, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending private message: {ex.Message}");
            }
        }

        // Room creation/deletion is intentionally NOT done through the
        // SignalR hub. ChatHub.CreateRoom/DeleteRoom are commented out on
        // the API side because room management goes through the REST
        // endpoints in RoomsController, which broadcast "RoomCreated" /
        // "RoomDeleted" to all connected clients afterwards. This class
        // used to expose CreateRoomAsync/DeleteRoomAsync methods that
        // invoked those nonexistent hub methods, which silently failed on
        // every call. Use ApiService.CreateRoomAsync / ApiService.DeleteRoomAsync
        // (REST) from ChatForm instead - see BtnCreateRoom_Click / BtnDeleteRoom_Click.

        public async Task GetRoomMessagesAsync(Guid roomId)
        {
            try
            {
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("GetRoomMessages", roomId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting room messages: {ex.Message}");
            }
        }

        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public async Task DisconnectAsync()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
