using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Core
{
    public class MyWebSocketContext : IMyWebSocketContext
    {
        private readonly WebSocketContext _webSocketContext;

        public MyWebSocketContext(WebSocketContext webSocketContext)
        {
            _webSocketContext = webSocketContext;
        }

        public IMyWebSocket WebSocket => new MyWebSocket();
    }
}
