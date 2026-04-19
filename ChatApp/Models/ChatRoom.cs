namespace ChatApp.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        
        // Relationships
        public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
