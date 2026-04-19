using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Models;
using System.Linq;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;
        private static Dictionary<string, string> _userConnections = new();
        private static Dictionary<string, string> _userNames = new();

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_userNames.TryGetValue(Context.ConnectionId, out var username))
            {
                _userNames.Remove(Context.ConnectionId);
                _userConnections.Remove(username);
                await Clients.All.SendAsync("UserDisconnected", username);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChat(string username)
        {
            _userConnections[username] = Context.ConnectionId;
            _userNames[Context.ConnectionId] = username;

            // Save user to database
            var user = new User
            {
                Username = username,
                ConnectionId = Context.ConnectionId,
                IsOnline = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UserJoined", username, GetOnlineUsers());
        }

        public async Task SendMessage(string message, int chatRoomId)
        {
            if (!_userNames.TryGetValue(Context.ConnectionId, out var username))
                return;

            var dbMessage = new Message
            {
                Content = message,
                ChatRoomId = chatRoomId,
                SentAt = DateTime.UtcNow
            };

            // Find or create user
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                dbMessage.UserId = user.Id;
                _context.Messages.Add(dbMessage);
                await _context.SaveChangesAsync();
            }

            await Clients.Group($"room-{chatRoomId}")
                .SendAsync("ReceiveMessage", username, message, DateTime.Now.ToString("HH:mm:ss"));
        }

        public async Task SendPrivateMessage(string toUsername, string message)
        {
            if (!_userNames.TryGetValue(Context.ConnectionId, out var fromUsername))
                return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            
            if (_userConnections.TryGetValue(toUsername, out var toConnectionId))
            {
                await Clients.Client(toConnectionId)
                    .SendAsync("ReceivePrivateMessage", fromUsername, message, timestamp);
                
                await Clients.Caller
                    .SendAsync("ReceivePrivateMessage", $"{fromUsername} (sent)", message, timestamp);
            }
        }

        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
            
            if (_userNames.TryGetValue(Context.ConnectionId, out var username))
            {
                await Clients.Group($"room-{roomId}")
                    .SendAsync("UserJoinedRoom", username);
            }
        }

        public async Task LeaveRoom(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room-{roomId}");
            
            if (_userNames.TryGetValue(Context.ConnectionId, out var username))
            {
                await Clients.Group($"room-{roomId}")
                    .SendAsync("UserLeftRoom", username);
            }
        }

        public List<string> GetOnlineUsers()
        {
            return _userConnections.Keys.ToList();
        }

        public async Task<List<string>> GetRoomMessages(int roomId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.SentAt)
                .Include(m => m.User)
                .Select(m => $"{m.User.Username}: {m.Content} ({m.SentAt:HH:mm:ss})")
                .ToListAsync();
            
            return messages;
        }
    }
}
