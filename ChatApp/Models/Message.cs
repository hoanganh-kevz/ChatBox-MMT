namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? SenderUsername { get; set; }
        public int UserId { get; set; }
        public int? ChatRoomId { get; set; }
        public int? ReceiverUserId { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime SentAt { get; set; }
        
        // Relationships
        public User? User { get; set; }
        public ChatRoom? ChatRoom { get; set; }
        public User? ReceiverUser { get; set; }
    }
}
