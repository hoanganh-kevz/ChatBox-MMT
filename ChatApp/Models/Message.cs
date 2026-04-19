namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int UserId { get; set; }
        public int ChatRoomId { get; set; }
        public DateTime SentAt { get; set; }
        
        // Relationships
        public User? User { get; set; }
        public ChatRoom? ChatRoom { get; set; }
    }
}
