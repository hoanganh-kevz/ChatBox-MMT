using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<ChatRoomMember>()
                .HasOne(m => m.User)
                .WithMany(u => u.ChatRoomMembers)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<ChatRoomMember>()
                .HasOne(m => m.ChatRoom)
                .WithMany(cr => cr.Members)
                .HasForeignKey(m => m.ChatRoomId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatRoom)
                .WithMany(cr => cr.Messages)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ReceiverUser)
                .WithMany()
                .HasForeignKey(m => m.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for fast queries
            modelBuilder.Entity<Message>()
                .HasIndex(m => m.ChatRoomId);

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.UserId);

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.ReceiverUserId);

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.IsPrivate);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed default rooms
            modelBuilder.Entity<ChatRoom>().HasData(
                new ChatRoom { Id = 1, Name = "General", Description = "Phòng chat chung cho tất cả mọi người", IsDefault = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedByUserId = 0 },
                new ChatRoom { Id = 2, Name = "Nhóm 1", Description = "Nhóm thảo luận 1", IsDefault = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedByUserId = 0 },
                new ChatRoom { Id = 3, Name = "Nhóm 2", Description = "Nhóm thảo luận 2", IsDefault = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedByUserId = 0 }
            );
        }
    }
}
