# ChatBox - Complete Chat Application Suite 💬

Repository chứa 3 dự án chat được phát triển từ UDP cơ bản đến Web Chat hoàn chỉnh.

## 📁 Cấu trúc dự án

```
MMT/
├── UDP/                 # UDP Chat (Ban đầu - Console)
│   ├── Client/         # UDP Client
│   └── Sever/          # UDP Server
│
├── TCP/                 # TCP Chat (Cải tiến)
│   ├── Client/         # TCP Client
│   └── Server/         # TCP Server
│
├── ChatApp/             # Web Chat Application (Hoàn chỉnh) ⭐
│   ├── Models/         # Database models
│   ├── Data/           # Entity Framework DbContext
│   ├── Hubs/           # SignalR Hubs
│   ├── Pages/          # Razor Pages + Chat UI
│   ├── README.md       # Tài liệu DetailedUD
│   ├── QUICKSTART.md   # Hướng dẫn nhanh
│   └── DEVELOPMENT_NOTES.md
│
└── README.md           # File này
```

---

## 🚀 Dự án chính: ChatApp (Web-based)

Đây là phiên bản hoàn chỉnh nhất - một ứng dụng chat web thời gian thực.

### ✨ Tính năng
- ✅ **Real-time messaging** với SignalR
- ✅ **Chat nhóm** (3 phòng chat)
- ✅ **Chat riêng** (1-1)
- ✅ **Lịch sử tin nhắn** (SQLite)
- ✅ **Danh sách người dùng online**
- ✅ **Giao diện đẹp** (HTML/CSS/JS)
- ✅ **Responsive design**

### 🛠️ Công nghệ
- **Backend**: ASP.NET Core 10.0
- **Real-time**: SignalR
- **Database**: SQLite + Entity Framework Core
- **Frontend**: Razor Pages + HTML/CSS/JavaScript

### ▶️ Chạy ChatApp

```bash
cd ChatApp
dotnet run
```

Truy cập: **http://localhost:5292**

[Chi tiết tại ChatApp/README.md](ChatApp/README.md)

---

## 📚 Dự án khác

### UDP Chat (Ban đầu)
- **Mô tả**: Chat Console sử dụng UDP protocol
- **Cách chạy**:
  ```bash
  # Terminal 1: Server
  cd UDP/Sever
  dotnet run
  
  # Terminal 2: Client
  cd UDP/Client
  dotnet run
  ```
- **Tính năng**: Chat 1-1, chuyển text thành UPPERCASE

### TCP Chat (Cải tiến)
- **Mô tả**: Chat Console sử dụng TCP protocol
- **Cách chạy**:
  ```bash
  # Terminal 1: Server
  cd TCP/Server
  dotnet run
  
  # Terminal 2: Client
  cd TCP/Client
  dotnet run
  ```
- **Tính năng**: Chat 1-1, kết nối ổn định

---

## 📋 So sánh các phiên bản

| Tính năng | UDP | TCP | Web Chat |
|-----------|-----|-----|----------|
| Giao diện | Console | Console | Web UI |
| Protocol | UDP | TCP | HTTP/WebSocket |
| Real-time | ✓ | ✓ | ✓ (SignalR) |
| Chat nhóm | ✗ | ✗ | ✓ |
| Chat riêng | ✗ | ✗ | ✓ |
| Lịch sử | ✗ | ✗ | ✓ |
| Database | ✗ | ✗ | ✓ |
| Multiple clients | 1 | 1 | ∞ |
| Online list | ✗ | ✗ | ✓ |
| Responsive | ✗ | ✗ | ✓ |

---

## 🔧 Yêu cầu hệ thống

- **.NET 10.0** SDK hoặc mới hơn
- **Windows/macOS/Linux**
- **Web Browser** (cho ChatApp)
- **Git** (optional)

### Cài đặt .NET
```bash
# Windows (winget)
winget install Microsoft.DotNet.SDK.10

# macOS (brew)
brew install dotnet-sdk

# Linux (apt)
sudo apt install dotnet-sdk-10.0
```

---

## 📝 Hướng dẫn Chi tiết

### Khởi động ChatApp (Được khuyến cáo)
1. **Mở Terminal** tại `ChatApp/`
2. **Chạy**: `dotnet run`
3. **Mở Browser**: `http://localhost:5292`
4. **Nhập tên** và bắt đầu chat!

### Testing với Multiple Users
1. Mở browser tab 1: Đăng nhập với "User1"
2. Mở browser tab 2: Đăng nhập với "User2"
3. Chat giữa 2 người hoặc trong phòng khác nhau

### Dừng ứng dụng
- Nhấn **Ctrl + C** trong terminal

---

## 🎓 Bài học rút ra

### Phát triển từng bước
```
UDP Protocol
    ↓
TCP Protocol
    ↓
HTTP/WebSocket (SignalR)
    ↓
Full Web Application
    ↓
Multi-user, Real-time, Persistent
```

### Công nghệ học được
- ✓ Network programming (UDP/TCP)
- ✓ ASP.NET Core
- ✓ SignalR (Real-time communication)
- ✓ Entity Framework Core (ORM)
- ✓ Database design (SQLite)
- ✓ Web development (HTML/CSS/JS)
- ✓ Responsive design
- ✓ Git & GitHub

---

## 📦 Deployment

### Local Development
```bash
dotnet run
```

### Production (Docker)
```bash
docker build -t chatapp .
docker run -p 80:5292 chatapp
```

### Cloud Deployment
- ☁️ Azure App Service
- ☁️ AWS EC2
- ☁️ Heroku
- ☁️ Railway

---

## 🐛 Troubleshooting

### Lỗi: Port đang được sử dụng
```bash
# Thay đổi port
dotnet run -- --urls="http://localhost:3000"
```

### Database error
```bash
# Xóa database cũ
rm ChatApp/chat.db

# Chạy lại (sẽ tạo mới)
dotnet run
```

### SignalR không kết nối
- Kiểm tra firewall
- Thử HTTP thay vì HTTPS
- Xem browser console (F12)

---

## 💡 Hướng phát triển tiếp theo

- [ ] User authentication (Login/Register)
- [ ] Encryption (Messages, Password)
- [ ] File sharing
- [ ] Image uploads
- [ ] Voice/Video calling
- [ ] Typing indicators
- [ ] Read receipts
- [ ] Mobile app (React Native/Flutter)
- [ ] Message search
- [ ] User profiles
- [ ] Admin dashboard

---

## 📄 License

Đây là dự án học tập cho mục đích giáo dục.

---

## 👨‍💻 Tác giả

Phát triển bởi: **hoanganh-kevz**

GitHub: https://github.com/hoanganh-kevz/ChatBox-MMT

---

## 🎉 Quick Links

- [ChatApp README](ChatApp/README.md) - Tài liệu đầy đủ
- [ChatApp QUICKSTART](ChatApp/QUICKSTART.md) - Hướng dẫn nhanh
- [Development Notes](ChatApp/DEVELOPMENT_NOTES.md) - Ghi chú phát triển

---

**Enjoy your Chat Applications! 🚀**

Bắt đầu với ChatApp ngay bây giờ:
```bash
cd ChatApp && dotnet run
```
