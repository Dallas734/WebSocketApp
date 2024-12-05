using Microsoft.AspNetCore.Mvc;
using ServerWebApi;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private static WebSocket _webSocket;
        private static bool _serverStarted = false;
        private static readonly ConcurrentDictionary<string, WebSocket> Clients = new();

        [HttpGet("connect")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var clientId = Guid.NewGuid().ToString();
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Clients.TryAdd(clientId, webSocket);

                Console.WriteLine($"Подключен новый клиент: {clientId}");

                try
                {
                    await ReceiveMessages(clientId, webSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка WebSocket (клиент {clientId}): {ex.Message}");
                }
                finally
                {
                    Clients.TryRemove(clientId, out _);
                    Console.WriteLine($"Соединение с клиентом {clientId} завершено.");
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                await HttpContext.Response.WriteAsync("Не является WebSocket-запросом.");
            }
        }

        /*private async Task StartWebSocketServer(string serverUrl, int port)
        {
            var httpListener = new System.Net.HttpListener();
            httpListener.Prefixes.Add($"http://{serverUrl}:{port}/");
            httpListener.Start();
            Console.WriteLine($"Сервер запущен на ws://{serverUrl}:{port}/");

            while (true)
            {
                var context = await httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    _webSocket = (await context.AcceptWebSocketAsync(null)).WebSocket;
                    Console.WriteLine("Подключен новый клиент.");
                    await ReceiveMessages(_webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }*/

        private async Task ReceiveMessages(string clientId, WebSocket webSocket)
        {
            var buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                    Console.WriteLine("Соединение закрыто.");
                    break;
                }
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Получено сообщение от клиента: {message}");
            }
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromQuery] string message)
        {
            if (_webSocket == null || _webSocket.State != WebSocketState.Open)
                return BadRequest("Нет активных подключений.");

            var buffer = Encoding.UTF8.GetBytes(message);
            _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            using (ZyfraContext db = new ZyfraContext())
            {
                Message msg = new Message() { Message1 = message };
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            return Ok($"Сообщение '{message}' отправлено клиенту.");
        }

        [HttpPost("stop")]
        public IActionResult StopServer()
        {
            _serverStarted = false;
            _webSocket?.Dispose();
            return Ok("Сервер остановлен.");
        }
    }
}
