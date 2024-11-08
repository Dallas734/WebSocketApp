// WebSocketApp.Server/WebSocketServer.cs
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketApp.Core;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Server
{
    public class WebSocketServer
    {
        private readonly IMyHttpListener _httpListener;
        private readonly string _serverUrl;
        private readonly int _port;

        public WebSocketServer(IMyHttpListener httpListener, string serverUrl, int port)
        {
            _httpListener = httpListener;
            _serverUrl = serverUrl;
            _port = port;
        }

        public async Task Start()
        {
            var fullAddress = $"http://{_serverUrl}:{_port}/";
            _httpListener.AddPrefix(fullAddress);
            _httpListener.Start();
            Console.WriteLine($"Сервер запущен на {fullAddress} и ожидает подключения...");

            while (true)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    var webSocketContext = await context.AcceptWebSocketAsync(null);
                    Console.WriteLine("Новое подключение установлено. Ожидание сообщений...");

                    await HandleClient(webSocketContext.WebSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        public async Task Stop()
        {
            _httpListener.Stop();
        }

        public async Task HandleClient(WebSocket webSocket)
        {
            var buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                    Console.WriteLine("Соединение закрыто клиентом.");
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Получено сообщение: " + message);

                var response = Encoding.UTF8.GetBytes("Сообщение принято: " + message);
                await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
