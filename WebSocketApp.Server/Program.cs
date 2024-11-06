// WebSocketApp.Server/Program.cs
using System;
using WebSocketApp.Core;
using WebSocketApp.Core.Interfaces;
using WebSocketApp.Server;

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

            IMyHttpListener httpListener = new MyHttpListener();
            var server = new WebSocketServer(httpListener, serverUrl, port);

            await server.Start();
        }
    }
}
