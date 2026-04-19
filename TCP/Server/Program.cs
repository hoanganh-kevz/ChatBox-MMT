using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Tcp Server";
            // giá trị Any của IPAddress tương ứng với Ip của tất cả các giao diện mạng trên máy
            var localIp = IPAddress.Any;
            // tiến trình server sẽ sử dụng cổng tcp 1308
            var localPort = 1308;
            // biến này sẽ chứa "địa chỉ" của tiến trình server trên mạng
            var localEndPoint = new IPEndPoint(localIp, localPort);
            // tcp sử dụng đồng thời hai socket: 
            // một socket để chờ nghe kết nối, một socket để gửi/nhận dữ liệu
            // socket listener này chỉ làm nhiệm vụ chờ kết nối từ Client
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // yêu cầu hệ điều hành cho phép chiếm dụng cổng tcp 1308
            // server sẽ nghe trên tất cả các mạng mà máy tính này kết nối tới
            // chỉ cần gói tin tcp đến cổng 1308, tiến trình server sẽ nhận được
            listener.Bind(localEndPoint);
            // bắt đầu lắng nghe chờ các gói tin tcp đến cổng 1308
            listener.Listen(10);
            Console.WriteLine($"Local socket bind to {localEndPoint}. Waiting for request ...");
            var size = 1024;
            var receiveBuffer = new byte[size];
            while (true)
            {
                // tcp đòi hỏi một socket thứ hai làm nhiệm vụ gửi/nhận dữ liệu
                // socket này được tạo ra bởi lệnh Accept
                var socket = listener.Accept();
                Console.WriteLine($"Accepted connection from {socket.RemoteEndPoint}");
                try
                {
                    // nhận dữ liệu vào buffer
                    var length = socket.Receive(receiveBuffer);
                    socket.Shutdown(SocketShutdown.Receive);
                    var text = Encoding.ASCII.GetString(receiveBuffer, 0, length);
                    Console.WriteLine($"Received: {text}");
                    // chuyển chuỗi thành dạng in hoa
                    var result = text.ToUpper();
                    var sendBuffer = Encoding.ASCII.GetBytes(result);
                    // gửi kết quả lại cho client
                    socket.Send(sendBuffer);
                    Console.WriteLine($"Sent: {result}");
                    socket.Shutdown(SocketShutdown.Send);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Socket error from {socket.RemoteEndPoint}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling {socket.RemoteEndPoint}: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine($"Closing connection from {socket.RemoteEndPoint}\r\n");
                    socket.Close();
                    Array.Clear(receiveBuffer, 0, size);
                }
            }
        }
    }
}