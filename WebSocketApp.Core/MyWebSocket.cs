using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Core
{
    public class MyWebSocket : IMyWebSocket
    {
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();

        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            await _webSocket.ConnectAsync(uri, cancellationToken);
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return await _webSocket.ReceiveAsync(buffer, cancellationToken);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public void Dispose()
        {
            _webSocket.Dispose();
        }

        public WebSocketState State { get; set; }
    }
}
