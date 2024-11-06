// WebSocketApp.Client/WebSocketClient.cs
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Client
{
    public class WebSocketClient
    {
        private readonly IMyWebSocket _webSocket;
        private readonly string _serverUrl;
        private readonly int _port;

        public WebSocketClient(IMyWebSocket clientWebSocket, string serverUrl, int port)
        {
            _webSocket = clientWebSocket;
            _serverUrl = serverUrl;
            _port = port;
        }

        public async Task ConnectAsync()
        {
            var fullAddress = $"ws://{_serverUrl}:{_port}/";
            Console.WriteLine($"Подключение к серверу {fullAddress}...");
            await _webSocket.ConnectAsync(new Uri(fullAddress), CancellationToken.None);
            Console.WriteLine("Подключение установлено.");
            _webSocket.State = WebSocketState.Open;
        }

        public async Task SendMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Сообщение отправлено: " + message);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var buffer = new byte[1024];
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine("Сообщение получено: " + receivedMessage);
            return receivedMessage;
        }

        public async Task DisconnectAsync()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
            Console.WriteLine("Соединение закрыто.");
        }
    }
}
