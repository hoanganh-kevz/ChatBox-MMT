using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Models;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;
        // Track connectionId -> username
        private static readonly Dictionary<string, string> _connectionToUser = new();
        // Track username -> connectionId
        private static readonly Dictionary<string, string> _userToConnection = new();

        // Avatar color palette
        private static readonly string[] _avatarColors = new[]
        {
            "#6366f1", "#8b5cf6", "#ec4899", "#f43f5e", "#f97316",
            "#eab308", "#22c55e", "#14b8a6", "#06b6d4", "#3b82f6",
            "#a855f7", "#d946ef", "#0ea5e9", "#10b981", "#f59e0b"
        };

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectionToUser.TryGetValue(Context.ConnectionId, out var username))
            {
                _connectionToUser.Remove(Context.ConnectionId);
                _userToConnection.Remove(username);

                // Update user status in DB
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user != null)
                {
                    user.IsOnline = false;
                    user.LastSeen = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                await Clients.All.SendAsync("UserDisconnected", username);
                await Clients.All.SendAsync("OnlineUsersUpdated", GetOnlineUsersList());
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChat(string username)
        {
            // Check if username already taken by another connection
            if (_userToConnection.ContainsKey(username) && _userToConnection[username] != Context.ConnectionId)
            {
                throw new HubException("Tên người dùng đã được sử dụng!");
            }

            _userToConnection[username] = Context.ConnectionId;
            _connectionToUser[Context.ConnectionId] = username;

            // Find or create user in DB
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                var random = new Random();
                user = new User
                {
                    Username = username,
                    ConnectionId = Context.ConnectionId,
                    IsOnline = true,
                    AvatarColor = _avatarColors[random.Next(_avatarColors.Length)],
                    CreatedAt = DateTime.UtcNow,
                    LastSeen = DateTime.UtcNow
                };
                _context.Users.Add(user);
            }
            else
            {
                user.ConnectionId = Context.ConnectionId;
                user.IsOnline = true;
                user.LastSeen = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            // Auto-join all default rooms
            var defaultRooms = await _context.ChatRooms.Where(r => r.IsDefault).ToListAsync();
            foreach (var room in defaultRooms)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{room.Id}");
            }

            await Clients.All.SendAsync("UserJoined", username);
            await Clients.All.SendAsync("OnlineUsersUpdated", GetOnlineUsersList());
        }

        public async Task SendMessage(string message, int chatRoomId)
        {
            if (!_connectionToUser.TryGetValue(Context.ConnectionId, out var username))
                return;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return;

            var dbMessage = new Message
            {
                Content = message,
                SenderUsername = username,
                UserId = user.Id,
                ChatRoomId = chatRoomId,
                IsPrivate = false,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(dbMessage);
            await _context.SaveChangesAsync();

            var messageData = new
            {
                id = dbMessage.Id,
                sender = username,
                content = message,
                time = dbMessage.SentAt.ToLocalTime().ToString("HH:mm"),
                avatarColor = user.AvatarColor ?? "#6366f1"
            };

            await Clients.Group($"room-{chatRoomId}")
                .SendAsync("ReceiveMessage", messageData);
        }

        public async Task SendPrivateMessage(string toUsername, string message)
        {
            if (!_connectionToUser.TryGetValue(Context.ConnectionId, out var fromUsername))
                return;

            var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == fromUsername);
            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == toUsername);
            if (fromUser == null || toUser == null) return;

            var dbMessage = new Message
            {
                Content = message,
                SenderUsername = fromUsername,
                UserId = fromUser.Id,
                ReceiverUserId = toUser.Id,
                IsPrivate = true,
                ChatRoomId = null,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(dbMessage);
            await _context.SaveChangesAsync();

            var messageData = new
            {
                id = dbMessage.Id,
                sender = fromUsername,
                content = message,
                time = dbMessage.SentAt.ToLocalTime().ToString("HH:mm"),
                avatarColor = fromUser.AvatarColor ?? "#6366f1"
            };

            // Send to receiver if online
            if (_userToConnection.TryGetValue(toUsername, out var toConnectionId))
            {
                await Clients.Client(toConnectionId)
                    .SendAsync("ReceivePrivateMessage", messageData);
            }

            // Send back to sender
            await Clients.Caller.SendAsync("ReceivePrivateMessage", messageData);
        }

        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
        }

        public async Task LeaveRoom(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room-{roomId}");
        }

        public async Task<object[]> GetRoomMessages(int roomId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatRoomId == roomId && !m.IsPrivate)
                .OrderBy(m => m.SentAt)
                .Include(m => m.User)
                .Take(100)
                .Select(m => new
                {
                    id = m.Id,
                    sender = m.SenderUsername ?? (m.User != null ? m.User.Username : "Unknown"),
                    content = m.Content,
                    time = m.SentAt.ToLocalTime().ToString("HH:mm"),
                    avatarColor = m.User != null ? m.User.AvatarColor ?? "#6366f1" : "#6366f1"
                })
                .ToArrayAsync();

            return messages.Select(m => (object)m).ToArray();
        }

        public async Task<object[]> GetPrivateMessages(string otherUsername)
        {
            if (!_connectionToUser.TryGetValue(Context.ConnectionId, out var currentUsername))
                return Array.Empty<object>();

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);
            var otherUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == otherUsername);
            if (currentUser == null || otherUser == null) return Array.Empty<object>();

            var messages = await _context.Messages
                .Where(m => m.IsPrivate &&
                    ((m.UserId == currentUser.Id && m.ReceiverUserId == otherUser.Id) ||
                     (m.UserId == otherUser.Id && m.ReceiverUserId == currentUser.Id)))
                .OrderBy(m => m.SentAt)
                .Include(m => m.User)
                .Take(100)
                .Select(m => new
                {
                    id = m.Id,
                    sender = m.SenderUsername ?? (m.User != null ? m.User.Username : "Unknown"),
                    content = m.Content,
                    time = m.SentAt.ToLocalTime().ToString("HH:mm"),
                    avatarColor = m.User != null ? m.User.AvatarColor ?? "#6366f1" : "#6366f1"
                })
                .ToArrayAsync();

            return messages.Select(m => (object)m).ToArray();
        }

        public async Task<object[]> GetRooms()
        {
            var rooms = await _context.ChatRooms
                .OrderBy(r => r.Id)
                .Select(r => new
                {
                    id = r.Id,
                    name = r.Name,
                    description = r.Description,
                    isDefault = r.IsDefault
                })
                .ToArrayAsync();

            return rooms.Select(r => (object)r).ToArray();
        }

        public async Task<object?> CreateRoom(string name, string description)
        {
            if (!_connectionToUser.TryGetValue(Context.ConnectionId, out var username))
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var room = new ChatRoom
            {
                Name = name,
                Description = description,
                IsDefault = false,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = user.Id
            };

            _context.ChatRooms.Add(room);
            await _context.SaveChangesAsync();

            var roomData = new
            {
                id = room.Id,
                name = room.Name,
                description = room.Description,
                isDefault = false
            };

            // Notify all clients
            await Clients.All.SendAsync("RoomCreated", roomData);

            return roomData;
        }

        public List<object> GetOnlineUsersList()
        {
            var onlineUsers = new List<object>();
            foreach (var kvp in _userToConnection)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == kvp.Key);
                onlineUsers.Add(new
                {
                    username = kvp.Key,
                    avatarColor = user?.AvatarColor ?? "#6366f1"
                });
            }
            return onlineUsers;
        }
    }
}
