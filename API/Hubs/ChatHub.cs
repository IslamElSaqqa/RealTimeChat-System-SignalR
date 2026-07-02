using AdvancedChat.API.Data;
using AdvancedChat.API.Services;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // Store online users
        private static readonly Dictionary<string, string> OnlineUsers = new();
        private readonly ApplicationDbContext _context;

        private readonly ConnectionManager _connectionManager;

        public ChatHub(ApplicationDbContext context, ConnectionManager connectionManager)
        {
            _context = context;
            _connectionManager = connectionManager;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier!;

            _connectionManager.Add(userId, Context.ConnectionId);

            var onlineUserNames = await _connectionManager.GetOnlineUserFullNamesAsync(_context);
            await Clients.All.SendAsync(
                "OnlineUsers",
                onlineUserNames);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier!;

            _connectionManager.Remove(userId);

            var onlineUserNames = await _connectionManager.GetOnlineUserFullNamesAsync(_context);
            await Clients.All.SendAsync(
                "OnlineUsers",
                onlineUserNames);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendRoomMessage(Guid roomId, string message)
        {
            var userId = Context.UserIdentifier!;

            var chatMessage = new Message
            {
                Content = message,
                SenderId = userId,
                RoomId = roomId,
                MessageType = MessageType.Public,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(chatMessage);

            await _context.SaveChangesAsync();

            var sender = await _context.Users.FindAsync(userId);

            await Clients.Group(roomId.ToString())
                .SendAsync(
                    "ReceiveRoomMessage",
                    sender!.FullName,
                    message,
                    chatMessage.SentAt);
        }

        public async Task JoinRoom(Guid roomId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                roomId.ToString());

            await Clients.Group(roomId.ToString())
                .SendAsync(
                    "UserJoined",
                    Context.UserIdentifier);
        }

        public async Task LeaveRoom(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                roomId.ToString());

            await Clients.Group(roomId.ToString())
                .SendAsync(
                    "UserLeft",
                    Context.UserIdentifier);
        }

        public async Task SendPrivateMessage(
            string receiverId,
            string message)
        {
            var senderId = Context.UserIdentifier!;

            var sender = await _context.Users.FindAsync(senderId);

            var chatMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                MessageType = MessageType.Private,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(chatMessage);

            await _context.SaveChangesAsync();

            var receiverConnection =
                _connectionManager.GetConnection(receiverId);

            if (receiverConnection != null)
            {
                await Clients.Client(receiverConnection)
                    .SendAsync(
                        "ReceivePrivateMessage",
                        sender!.FullName,
                        message,
                        chatMessage.SentAt);
            }
        }

        //public async Task CreateRoom(string roomName)
        //{
        //    var room = new Room
        //    {
        //        Name = roomName,
        //        CreatedById = Context.UserIdentifier!,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _context.Rooms.Add(room);

        //    await _context.SaveChangesAsync();

        //    await Clients.All.SendAsync(
        //        "RoomCreated",
        //        room.Id,
        //        room.Name);
        //}

        //public async Task DeleteRoom(Guid roomId)
        //{
        //    var room = await _context.Rooms.FindAsync(roomId);

        //    if (room == null)
        //        return;

        //    _context.Rooms.Remove(room);

        //    await _context.SaveChangesAsync();

        //    await Clients.All.SendAsync(
        //        "RoomDeleted",
        //        roomId);
        //}

        public async Task GetRoomMessages(Guid roomId)
        {
            var messages = await _context.Messages
                .Where(m => m.RoomId == roomId)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    Sender = m.Sender.FullName,
                    m.SentAt
                })
                .ToListAsync();

            await Clients.Caller.SendAsync("RoomMessagesLoaded", messages);
        }
    }
}