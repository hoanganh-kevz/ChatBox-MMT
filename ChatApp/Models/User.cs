namespace ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? ConnectionId { get; set; }
        public bool IsOnline { get; set; }
        public string? AvatarColor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
        
        // Relationships
        public ICollection<ChatRoomMember> ChatRoomMembers { get; set; } = new List<ChatRoomMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
