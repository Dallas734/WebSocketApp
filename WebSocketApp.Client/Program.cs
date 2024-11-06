// WebSocketApp.Client/Program.cs
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketApp.Client;
using WebSocketApp.Core;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Введите сервер (например, localhost): ");
            string serverUrl = Console.ReadLine();

            Console.Write("Введите порт (например, 8080): ");
            int port = int.Parse(Console.ReadLine());

            IMyWebSocket webSocket = new MyWebSocket();
            var client = new WebSocketClient(webSocket, serverUrl, port);

            await client.ConnectAsync();

            // Пример обмена сообщениями
            while (true)
            {
                Console.Write("Введите сообщение (или 'exit' для завершения): ");
                string message = Console.ReadLine();

                if (message.ToLower() == "exit")
                {
                    await client.DisconnectAsync();
                    break;
                }

                await client.SendMessageAsync(message);
                // await client.ReceiveMessageAsync();
            }
        }
    }
}
