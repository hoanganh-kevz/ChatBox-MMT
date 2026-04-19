using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Tcp Client";
            // yêu cầu người dùng nhập ip của server
            Console.Write("Server IP address: ");
            var serverIpStr = Console.ReadLine();
            // chuyển đổi chuỗi ký tự thành object thuộc kiểu IPAddress
            var serverIp = IPAddress.Parse(serverIpStr);
            // yêu cầu người dùng nhập cổng của server
            Console.Write("Server port: ");
            var serverPortStr = Console.ReadLine();
            // chuyển chuỗi ký tự thành biến kiểu int
            var serverPort = int.Parse(serverPortStr);
            // đây là "địa chỉ" của tiến trình server trên mạng
            // mỗi endpoint chứa ip của host và port của tiến trình
            var serverEndpoint = new IPEndPoint(serverIp, serverPort);
            var size = 1024; // kích thước của bộ đệm
            var receiveBuffer = new byte[size]; // mảng byte làm bộ đệm            
            while (true)
            {
                // yêu cầu người dùng nhập một chuỗi
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("# Text >>> ");
                Console.ResetColor();
                var text = Console.ReadLine();
                if (text is null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }
                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                // khởi tạo object của lớp socket để sử dụng dịch vụ Tcp
                // lưu ý SocketType của Tcp là Stream 
                using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                // tạo kết nối tới Server
                socket.Connect(serverEndpoint);
                // biến đổi chuỗi thành mảng byte
                var sendBuffer = Encoding.ASCII.GetBytes(text);
                // gửi mảng byte trên đến tiến trình server
                socket.Send(sendBuffer);
                // không tiếp tục gửi dữ liệu nữa
                socket.Shutdown(SocketShutdown.Send);
                // nhận mảng byte từ dịch vụ Tcp và lưu vào bộ đệm                    
                var length = socket.Receive(receiveBuffer);
                // chuyển đổi mảng byte về chuỗi
                var result = Encoding.ASCII.GetString(receiveBuffer, 0, length);
                // xóa bộ đệm (để lần sau sử dụng cho yên tâm)
                Array.Clear(receiveBuffer, 0, size);
                // không tiếp tục nhận dữ liệu nữa
                socket.Shutdown(SocketShutdown.Receive);
                // đóng socket và giải phóng tài nguyên
                // in kết quả ra màn hình
                Console.WriteLine($">>> {result}");
            }
        }
    }
}