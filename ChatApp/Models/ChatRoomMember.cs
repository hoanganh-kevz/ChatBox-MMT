namespace ChatApp.Models
{
    public class ChatRoomMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatRoomId { get; set; }
        public DateTime JoinedAt { get; set; }
        
        // Relationships
        public User? User { get; set; }
        public ChatRoom? ChatRoom { get; set; }
    }
}
