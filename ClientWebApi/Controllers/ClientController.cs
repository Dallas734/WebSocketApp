using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketApp.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketClientController : ControllerBase
    {
        private static ClientWebSocket _clientWebSocket;
        private static bool _isConnected = false;

        [HttpPost("connect")]
        public async Task<IActionResult> Connect([FromQuery] string serverUrl, [FromQuery] int port)
        {
            if (_isConnected)
                return BadRequest("Клиент уже подключен.");

            var uri = new Uri($"ws://{serverUrl}:{port}/");
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);

            _isConnected = true;
            Console.WriteLine($"Подключено к серверу: ws://{serverUrl}:{port}");

            // Запускаем задачу для получения сообщений от сервера
            Task.Run(() => ReceiveMessages(_clientWebSocket));

            return Ok("Подключение установлено.");
        }

        private async Task ReceiveMessages(ClientWebSocket clientWebSocket)
        {
            var buffer = new byte[1024];

            while (clientWebSocket.State == WebSocketState.Open)
            {
                var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                    Console.WriteLine("Соединение закрыто.");
                    _isConnected = false;
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Получено сообщение от сервера: {message}");
            }
        }

        [HttpPost("disconnect")]
        public IActionResult Disconnect()
        {
            if (_clientWebSocket != null && _clientWebSocket.State == WebSocketState.Open)
            {
                _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None).Wait();
                _isConnected = false;
            }
            return Ok("Отключено от сервера.");
        }
    }
}
