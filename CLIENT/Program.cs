using System.Net.Sockets;
using System.Text;

namespace CLIENT
{
    internal class Program
    {
        private static string username = "god";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите ваш ник");
            username = Console.ReadLine();
            TcpClient client = new TcpClient();
            string path = "127.0.0.1";
            try
            {
                await client.ConnectAsync(path, 7777);
            }
            catch
            {
                Console.WriteLine("Не подключились");
                return;
            }

            if (client.Connected)
            {
                Console.WriteLine("Connected Done...");
            }
            else
            {
                Console.WriteLine("False connected...");
            }

            _ = Task.Run(() => ReceiveMessage(client));

            NetworkStream stream = client.GetStream();
            while (true)
            {
                string input = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(username + " : " + input); // Передаем сообщение на сервер как стрингу
                await stream.WriteAsync(data, 0, data.Length);
            }
        }


        static async Task ReceiveMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int byteRead = await stream.ReadAsync(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, byteRead);
                Console.WriteLine($"[Server]: {message}");
            }
        }
    }
}

