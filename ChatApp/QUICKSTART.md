# Hướng dẫn cài đặt và chạy Chat Application

## Các bước nhanh để chạy ứng dụng

### 1. Mở Terminal/PowerShell

Trong VS Code hoặc Command Prompt, chuyển đến thư mục dự án:

```powershell
cd C:\MMT\ChatApp
```

### 2. Chạy ứng dụng

```bash
dotnet run
```

Hoặc nếu bạn muốn chỉ định port khác:

```bash
dotnet run -- --urls="http://localhost:3000"
```

### 3. Mở trình duyệt

Truy cập vào: **http://localhost:5292**

Bạn sẽ thấy màn hình đăng nhập.

---

## Testing với Multiple Users

Để test chat với nhiều người dùng:

1. **Mở tab/cửa sổ trình duyệt 1**: `http://localhost:5292`
   - Nhập username: `User1`
   - Nhấn "Tham gia Chat"

2. **Mở tab/cửa sổ trình duyệt 2**: `http://localhost:5292`
   - Nhập username: `User2`
   - Nhấn "Tham gia Chat"

3. **Test các tính năng**:
   - Gửi tin nhắn trong các phòng khác nhau
   - Chat riêng giữa 2 người dùng
   - Xem lịch sử tin nhắn

---

## Dừng ứng dụng

Nhấn **Ctrl + C** trong terminal để dừng server.

---

## Cấu hình lại Database

Nếu muốn reset database:

1. Dừng ứng dụng (Ctrl + C)
2. Xóa file `chat.db` trong thư mục `C:\MMT\ChatApp`
3. Chạy lại: `dotnet run`
4. Database sẽ được tạo lại từ đầu

---

## Cấu hình Custom Port

Mở file `Properties\launchSettings.json` và thay đổi:

```json
"applicationUrl": "https://localhost:7292;http://localhost:5292"
```

---

## Các vấn đề thường gặp

| Vấn đề | Giải pháp |
|--------|----------|
| Port đang bị sử dụng | Thay đổi port hoặc tìm process đang dùng port |
| Chat không hoạt động | Kiểm tra browser console (F12) để xem lỗi |
| Database error | Xóa `chat.db` và chạy lại |
| SignalR không kết nối | Kiểm tra firewall hoặc thử HTTPS |

---

## Requirements

- **.NET 10.0** hoặc mới hơn
- **Web Browser** (Chrome, Firefox, Edge, Safari)
- **Windows/macOS/Linux**

---

## Enjoy your Chat Application! 🎉

Bây giờ bạn có một ứng dụng chat web hoàn chỉnh với:
✅ Real-time messaging
✅ Multiple chat rooms
✅ Private messages
✅ Message history
✅ Online user list
✅ Beautiful UI
