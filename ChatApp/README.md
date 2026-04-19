# 💬 Chat Application - Web-based Real-time Messaging

## Mô tả

Đây là một ứng dụng chat web hoàn chỉnh được xây dựng bằng **ASP.NET Core 10.0** với **SignalR** cho giao tiếp real-time. Ứng dụng hỗ trợ chat 1-1 riêng tư, chat nhóm (chat room), lịch sử tin nhắn, và danh sách người dùng online.

## Tính năng

### ✨ Tính năng chính:
- **Chat Real-time**: Gửi và nhận tin nhắn ngay lập tức sử dụng SignalR
- **Chat Riêng (1-1)**: Chat trực tiếp với người dùng khác
- **Chat Nhóm**: Tham gia các phòng chat khác nhau
  - 📌 Tổng quát (General)
  - 👥 Nhóm 1
  - 👥 Nhóm 2
- **Danh sách người dùng Online**: Xem ai đang online trong hệ thống
- **Lịch sử tin nhắn**: Tất cả tin nhắn được lưu trong database SQLite
- **Giao diện hiện đại**: UI đẹp với gradient colors và animations
- **Responsive Design**: Hoạt động tốt trên desktop và mobile

## Công nghệ sử dụng

| Công nghệ | Mô tả |
|-----------|-------|
| **ASP.NET Core 10.0** | Framework web |
| **SignalR** | Real-time communication |
| **Entity Framework Core** | ORM cho database |
| **SQLite** | Database |
| **Razor Pages** | Server-side templates |
| **JavaScript** | Client-side logic |
| **HTML/CSS** | Giao diện người dùng |

## Cấu trúc dự án

```
ChatApp/
├── Models/
│   ├── User.cs           # Mô hình người dùng
│   ├── ChatRoom.cs       # Mô hình phòng chat
│   ├── ChatRoomMember.cs # Thành viên phòng chat
│   └── Message.cs        # Mô hình tin nhắn
├── Data/
│   └── ChatDbContext.cs  # Database context
├── Hubs/
│   └── ChatHub.cs        # SignalR Hub cho logic chat
├── Pages/
│   ├── Chat.cshtml       # Trang chat chính
│   ├── Chat.cshtml.cs    # Code-behind
│   ├── Index.cshtml      # Trang chủ (redirect to chat)
│   └── Shared/           # Layout chung
├── Properties/
│   └── launchSettings.json # Cài đặt launch
├── Program.cs            # Cấu hình startup
└── ChatApp.csproj        # Project file
```

## Cách chạy ứng dụng

### Yêu cầu hệ thống:
- .NET 10.0 SDK hoặc mới hơn
- Windows, macOS, hoặc Linux

### Các bước chạy:

1. **Clone hoặc tải dự án**
   ```bash
   cd C:\MMT\ChatApp
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Chạy ứng dụng**
   ```bash
   dotnet run
   ```

4. **Mở trình duyệt**
   - Truy cập: `http://localhost:5292`
   - Hoặc HTTPS: `https://localhost:7292`

5. **Sử dụng ứng dụng**
   - Nhập tên người dùng và nhấn "Tham gia Chat"
   - Gửi tin nhắn trong phòng chat
   - Chọn người dùng khác để chat riêng
   - Chuyển giữa các phòng chat bằng các nút tabs

## Hướng dẫn sử dụng

### 1. Đăng nhập
![Login Screen]
- Nhập tên người dùng của bạn
- Nhấn nút "Tham gia Chat"

### 2. Giao diện chính
Giao diện được chia thành 3 phần:

**Sidebar (bên trái)**
- Danh sách người dùng online
- Nút "Riêng" để chat 1-1 với người dùng bằng cách nhập tên họ

**Chat Area (giữa)**
- Hiển thị các tin nhắn
- Tabs để chuyển giữa các phòng chat
- Input field để gửi tin nhắn

**Input Area (dưới)**
- Nhập tin nhắn của bạn
- Nhấn "Gửi" hoặc Enter để gửi

### 3. Chat Nhóm (Chat Room)
- Nhấn vào các nút tabs: "📌 Tổng quát", "👥 Nhóm 1", "👥 Nhóm 2"
- Tất cả tin nhắn gửi đi sẽ được nhìn thấy bởi tất cả thành viên trong phòng
- Lịch sử tin nhắn được lưu và hiển thị khi bạn quay lại phòng

### 4. Chat Riêng (Private Message)
- Nhập tên người dùng vào ô "Người dùng..."
- Nhấn nút "Riêng"
- Gửi tin nhắn - nó sẽ chỉ được người dùng đó nhìn thấy

## API SignalR

### Server → Client:
| Method | Tham số | Mô tả |
|--------|---------|-------|
| `UserConnected` | `connectionId` | Người dùng vừa kết nối |
| `UserJoined` | `username, onlineUsers` | Người dùng tham gia chat |
| `UserDisconnected` | `username` | Người dùng ngắt kết nối |
| `ReceiveMessage` | `sender, message, time` | Nhận tin nhắn công khai |
| `ReceivePrivateMessage` | `sender, message, time` | Nhận tin nhắn riêng |
| `UserJoinedRoom` | `username` | Người dùng tham gia phòng |
| `UserLeftRoom` | `username` | Người dùng rời phòng |

### Client → Server:
| Method | Tham số | Mô tả |
|--------|---------|-------|
| `JoinChat` | `username` | Đăng nhập vào chat |
| `SendMessage` | `message, chatRoomId` | Gửi tin nhắn công khai |
| `SendPrivateMessage` | `toUsername, message` | Gửi tin nhắn riêng |
| `JoinRoom` | `roomId` | Tham gia phòng chat |
| `LeaveRoom` | `roomId` | Rời phòng chat |
| `GetOnlineUsers` | - | Lấy danh sách người dùng online |
| `GetRoomMessages` | `roomId` | Lấy lịch sử tin nhắn |

## Database Schema

### Users
```sql
- Id (int, PK)
- Username (string)
- ConnectionId (string)
- IsOnline (bool)
- CreatedAt (datetime)
```

### ChatRooms
```sql
- Id (int, PK)
- Name (string)
- Description (string)
- CreatedAt (datetime)
- CreatedByUserId (int, FK)
```

### Messages
```sql
- Id (int, PK)
- Content (string)
- UserId (int, FK)
- ChatRoomId (int, FK)
- SentAt (datetime)
```

### ChatRoomMembers
```sql
- Id (int, PK)
- UserId (int, FK)
- ChatRoomId (int, FK)
- JoinedAt (datetime)
```

## Tính năng bổ sung (có thể phát triển)

- [ ] Xác thực người dùng (Login/Register)
- [ ] Upload hình ảnh/file
- [ ] Emoji picker
- [ ] Typing indicator
- [ ] Read receipts
- [ ] Voice/Video call
- [ ] Search tin nhắn
- [ ] Message reactions
- [ ] User profiles
- [ ] Admin panel

## Troubleshooting

### Lỗi: "Couldn't find a project to run"
**Giải pháp**: Đảm bảo bạn đang ở thư mục `C:\MMT\ChatApp` hoặc sử dụng:
```bash
dotnet run --project "C:\MMT\ChatApp\ChatApp.csproj"
```

### Lỗi: "Port 5292 đang được sử dụng"
**Giải pháp**: Thay đổi port trong `launchSettings.json` hoặc:
```bash
dotnet run -- --urls="http://localhost:PORT"
```

### Database không tạo được
**Giải pháp**: Xóa file `chat.db` và chạy lại ứng dụng

## Performance Tips

- Ứng dụng hỗ trợ tối đa ~100+ concurrent users trên một máy server
- SignalR tự động fallback sang Long Polling nếu WebSocket không khả dụng
- SQLite phù hợp cho development, sử dụng SQL Server/PostgreSQL cho production

## License

Đây là dự án học tập cho mục đích giáo dục.

## Tác giả

Phát triển từ chương trình UDP cơ bản thành ứng dụng chat web hoàn chỉnh.

---

**Chúc bạn sử dụng ứng dụng chat vui vẻ! 🎉**
