using Desktop.Services;
using Desktop.Models;
using Desktop.Helpers;

namespace Desktop.Forms
{
    public partial class ChatForm : Form
    {
        private ApiService? _apiService;
        private SignalRService? _signalRService;
        private Guid _currentRoomId = Guid.Empty;
        private string? _currentPrivateUserId;
        private List<RoomModel> _rooms = new();
        private List<UserModel> _users = new();
        private Dictionary<Guid, List<MessageModel>> _roomMessages = new();
        private Dictionary<string, List<MessageModel>> _privateMessages = new();

        // UI Controls
        private ListBox? _lstRooms;
        private ListBox? _lstMessages;
        private ListBox? _lstOnlineUsers;
        private TextBox? _txtMessage;
        private Button? _btnSend;
        private Button? _btnCreateRoom;
        private Button? _btnDeleteRoom;
        private TextBox? _txtRoomName;
        private Label? _lblCurrentChat;
        private Label? _lblConnectionStatus;
        private Button? _btnLogout;

        public ChatForm()
        {
            this.Text = "Advanced Chat";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            this.Icon = null;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Left panel - Rooms
            var pnlRooms = new Panel 
            { 
                Dock = DockStyle.Left,
                Width = 240,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            var lblRoomsHeader = new Label 
            { 
                Text = "Rooms", 
                Font = new Font("Arial", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 0, 0, 5)
            };

            var pnlRoomInput = new Panel 
            { 
                Dock = DockStyle.Top,
                Height = 40,
                Margin = new Padding(0, 0, 0, 5)
            };

            _txtRoomName = new TextBox 
            { 
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 5, 0)
            };

            _btnCreateRoom = new Button 
            { 
                Text = "Create",
                Dock = DockStyle.Right,
                Width = 60,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            pnlRoomInput.Controls.Add(_btnCreateRoom);
            pnlRoomInput.Controls.Add(_txtRoomName);

            _lstRooms = new ListBox 
            { 
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                Margin = new Padding(0, 0, 0, 5)
            };

            _btnDeleteRoom = new Button 
            { 
                Text = "Delete Room",
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            pnlRooms.Controls.Add(_btnDeleteRoom);
            pnlRooms.Controls.Add(_lstRooms);
            pnlRooms.Controls.Add(pnlRoomInput);
            pnlRooms.Controls.Add(lblRoomsHeader);

            // Right panel - Online Users
            var pnlUsers = new Panel 
            { 
                Dock = DockStyle.Right,
                Width = 220,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            var lblUsersHeader = new Label 
            { 
                Text = "Online Users",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 0, 0, 10)
            };

            _lstOnlineUsers = new ListBox 
            { 
                Dock = DockStyle.Fill,
                IntegralHeight = false
            };

            pnlUsers.Controls.Add(_lstOnlineUsers);
            pnlUsers.Controls.Add(lblUsersHeader);

            // Center panel - Messages
            var pnlChat = new Panel 
            { 
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            _lblCurrentChat = new Label 
            { 
                Text = "Select a room",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 0, 0, 10)
            };

            _lstMessages = new ListBox 
            { 
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                Margin = new Padding(0, 0, 0, 10)
            };

            var pnlMessageInput = new Panel 
            { 
                Dock = DockStyle.Bottom,
                Height = 50
            };

            _txtMessage = new TextBox 
            { 
                Dock = DockStyle.Fill,
                Multiline = false,
                Margin = new Padding(0, 0, 5, 0)
            };

            _btnSend = new Button 
            { 
                Text = "Send",
                Dock = DockStyle.Right,
                Width = 75,
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            pnlMessageInput.Controls.Add(_btnSend);
            pnlMessageInput.Controls.Add(_txtMessage);

            pnlChat.Controls.Add(pnlMessageInput);
            pnlChat.Controls.Add(_lstMessages);
            pnlChat.Controls.Add(_lblCurrentChat);

            // Bottom panel - Status
            var pnlBottom = new Panel 
            { 
                Dock = DockStyle.Bottom,
                Height = 60,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10)
            };

            _lblConnectionStatus = new Label 
            { 
                Text = "Status: Disconnected",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _btnLogout = new Button 
            { 
                Text = "Logout",
                Dock = DockStyle.Right,
                Width = 100,
                BackColor = Color.FromArgb(111, 112, 114),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 0, 0, 0)
            };

            pnlBottom.Controls.Add(_btnLogout);
            pnlBottom.Controls.Add(_lblConnectionStatus);

            // Add panels to form
            this.Controls.Add(pnlChat);
            this.Controls.Add(pnlUsers);
            this.Controls.Add(pnlRooms);
            this.Controls.Add(pnlBottom);

            // Event handlers
            _btnCreateRoom!.Click += BtnCreateRoom_Click;
            _btnDeleteRoom!.Click += BtnDeleteRoom_Click;
            _btnSend!.Click += BtnSend_Click;
            _btnLogout!.Click += BtnLogout_Click;
            _lstRooms!.SelectedIndexChanged += LstRooms_SelectedIndexChanged;
            _lstOnlineUsers!.DoubleClick += LstOnlineUsers_DoubleClick;
            _txtMessage!.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return && e.Modifiers != Keys.Shift)
                {
                    BtnSend_Click(_btnSend, EventArgs.Empty);
                    e.Handled = true;
                }
            };

            this.ResumeLayout();
        }

        private void LstOnlineUsers_DoubleClick(object? sender, EventArgs e)
        {
            if (_lstOnlineUsers?.SelectedItem is UserModel user)
            {
                _currentPrivateUserId = user.Id;
                _currentRoomId = Guid.Empty;
                _lblCurrentChat!.Text = $"Private Chat with {user.FullName}";
                DisplayPrivateConversation(user.Id);
            }
        }

        private void DisplayPrivateConversation(string userId)
        {
            _lstMessages!.Items.Clear();
            if (_privateMessages.TryGetValue(userId, out var messages))
            {
                foreach (var msg in messages)
                {
                    _lstMessages.Items.Add($"{msg.Sender} ({msg.SentAt:HH:mm}): {msg.Content}");
                }
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TokenStorage.ClearAll();
                _signalRService?.DisconnectAsync().Wait();
                this.Close();
            }
        }

        private void LstRooms_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_lstRooms?.SelectedItem is RoomModel room)
            {
                SelectRoom(room.Id, room.Name);
            }
        }

        private void SelectRoom(Guid roomId, string roomName)
        {
            _currentRoomId = roomId;
            _currentPrivateUserId = null;
            _lblCurrentChat!.Text = roomName;
            DisplayRoomMessages(roomId);
            _signalRService?.JoinRoomAsync(roomId);
        }

        private void DisplayRoomMessages(Guid roomId)
        {
            _lstMessages!.Items.Clear();
            if (_roomMessages.TryGetValue(roomId, out var messages))
            {
                foreach (var msg in messages)
                {
                    _lstMessages.Items.Add($"{msg.Sender} ({msg.SentAt:HH:mm}): {msg.Content}");
                }
            }
        }

        private async void BtnCreateRoom_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtRoomName?.Text))
            {
                MessageBox.Show("Please enter a room name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Room creation/deletion is handled by the REST API
                // (RoomsController), which broadcasts "RoomCreated" over
                // SignalR to all clients afterwards. The ChatHub's own
                // CreateRoom/DeleteRoom methods are intentionally disabled
                // (commented out) to avoid creating rooms twice, so calling
                // _signalRService.CreateRoomAsync() here invoked a hub
                // method that no longer exists and silently failed every
                // time. Use _apiService instead, matching how the Web
                // client creates rooms.
                var newRoom = await _apiService!.CreateRoomAsync(_txtRoomName.Text);
                if (newRoom == null)
                {
                    MessageBox.Show("Failed to create room. It may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _txtRoomName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating room: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDeleteRoom_Click(object? sender, EventArgs e)
        {
            if (_currentRoomId == Guid.Empty)
            {
                MessageBox.Show("Please select a room first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this room?", "Delete Room", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    // Same reasoning as room creation: DeleteRoom is a REST
                    // endpoint (RoomsController), not a SignalR hub method.
                    var deleted = await _apiService!.DeleteRoomAsync(_currentRoomId);
                    if (!deleted)
                    {
                        MessageBox.Show("Failed to delete room.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting room: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtMessage?.Text))
            {
                return;
            }

            try
            {
                if (_currentRoomId != Guid.Empty)
                {
                    await _signalRService!.SendRoomMessageAsync(_currentRoomId, _txtMessage.Text);
                }
                else if (_currentPrivateUserId != null)
                {
                    await _signalRService!.SendPrivateMessageAsync(_currentPrivateUserId, _txtMessage.Text);
                }

                _txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                _apiService = new ApiService();
                _signalRService = new SignalRService();

                // Load rooms
                var rooms = await _apiService.GetRoomsAsync();
                if (rooms != null)
                {
                    _rooms = rooms;
                    _lstRooms!.Items.Clear();
                    foreach (var room in rooms)
                    {
                        _lstRooms.Items.Add(room);
                        _roomMessages[room.Id] = new List<MessageModel>();
                    }
                }

                // Load users
                var users = await _apiService.GetUsersAsync();
                if (users != null)
                {
                    _users = users;
                    UpdateOnlineUsersList(users.Where(u => u.IsOnline).Select(u => u.FullName).ToList());
                }

                // Subscribe to SignalR events
                _signalRService.OnRoomMessageReceived += (sender, message, time) =>
                {
                    if (_currentRoomId != Guid.Empty && _roomMessages.TryGetValue(_currentRoomId, out var messages))
                    {
                        messages.Add(new MessageModel
                        {
                            Content = message,
                            Sender = sender,
                            SentAt = time,
                            RoomId = _currentRoomId,
                            MessageType = 1
                        });
                        this.Invoke(() => DisplayRoomMessages(_currentRoomId));
                    }
                };

                _signalRService.OnPrivateMessageReceived += (sender, message, time) =>
                {
                    var senderUser = _users.FirstOrDefault(u => u.FullName == sender);
                    if (senderUser != null)
                    {
                        if (!_privateMessages.ContainsKey(senderUser.Id))
                        {
                            _privateMessages[senderUser.Id] = new List<MessageModel>();
                        }
                        _privateMessages[senderUser.Id].Add(new MessageModel
                        {
                            Content = message,
                            Sender = sender,
                            SentAt = time,
                            ReceiverId = senderUser.Id,
                            MessageType = 2
                        });
                        if (_currentPrivateUserId == senderUser.Id)
                        {
                            this.Invoke(() => DisplayPrivateConversation(senderUser.Id));
                        }
                    }
                };

                _signalRService.OnOnlineUsersUpdated += (users) =>
                {
                    this.Invoke(() => UpdateOnlineUsersList(users));
                };

                _signalRService.OnRoomCreated += (roomId, roomName) =>
                {
                    this.Invoke(() =>
                    {
                        var newRoom = new RoomModel { Id = roomId, Name = roomName, CreatedAt = DateTime.Now };
                        _rooms.Add(newRoom);
                        _lstRooms!.Items.Add(newRoom);
                        _roomMessages[roomId] = new List<MessageModel>();
                    });
                };

                _signalRService.OnRoomDeleted += (roomId) =>
                {
                    this.Invoke(() =>
                    {
                        _rooms = _rooms.Where(r => r.Id != roomId).ToList();
                        _lstRooms!.Items.Clear();
                        foreach (var room in _rooms)
                        {
                            _lstRooms.Items.Add(room);
                        }
                        if (_currentRoomId == roomId)
                        {
                            _currentRoomId = Guid.Empty;
                            _lblCurrentChat!.Text = "Select a room";
                            _lstMessages!.Items.Clear();
                        }
                    });
                };

                _signalRService.OnConnected += () =>
                {
                    this.Invoke(() =>
                    {
                        _lblConnectionStatus!.Text = "Status: Connected";
                        _lblConnectionStatus.ForeColor = Color.Green;
                    });
                };

                _signalRService.OnDisconnected += () =>
                {
                    this.Invoke(() =>
                    {
                        _lblConnectionStatus!.Text = "Status: Disconnected";
                        _lblConnectionStatus.ForeColor = Color.Red;
                    });
                };

                // Connect to SignalR AFTER all event handlers above are
                // wired up. ChatHub.OnConnectedAsync broadcasts "OnlineUsers"
                // to everyone (including this new connection) the instant
                // the connection is established. If we connect before
                // subscribing to OnOnlineUsersUpdated, that first broadcast
                // fires into an event with zero subscribers and is lost -
                // this client's online users list then stays stuck at
                // whatever GetUsersAsync() returned (server IsOnline flag,
                // which is not kept live), and it silently misses every
                // "someone connected/disconnected" update sent before this
                // point too. Subscribing first guarantees we never miss it.
                await _signalRService.ConnectAsync();
                _lblConnectionStatus!.Text = "Status: Connected";
                _lblConnectionStatus.ForeColor = Color.Green;

                // Load initial messages for rooms
                foreach (var room in _rooms)
                {
                    var messages = await _apiService.GetRoomMessagesAsync(room.Id);
                    if (messages != null)
                    {
                        _roomMessages[room.Id] = messages;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOnlineUsersList(List<string> onlineUsers)
        {
            _lstOnlineUsers!.Items.Clear();
            foreach (var user in _users.Where(u => onlineUsers.Contains(u.FullName)))
            {
                user.IsOnline = true;
                _lstOnlineUsers.Items.Add(user);
            }
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_signalRService != null)
            {
                await _signalRService.DisconnectAsync();
            }
        }
    }
}

