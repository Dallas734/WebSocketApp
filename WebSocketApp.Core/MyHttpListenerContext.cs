using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Core
{
    public class MyHttpListenerContext : IMyHttpListenerContext
    {
        private readonly HttpListenerContext _context;

        public MyHttpListenerContext(HttpListenerContext context)
        {
            _context = context;
        }

        public bool IsWebSocketRequest => _context.Request.IsWebSocketRequest;

        public HttpListenerRequest Request => _context.Request;

        public HttpListenerResponse Response => _context.Response;

        public async Task<IMyWebSocketContext> AcceptWebSocketAsync(string subProtocol)
        {
            var webSocketContext = await _context.AcceptWebSocketAsync(subProtocol);
            return new MyWebSocketContext(webSocketContext);
        }
    }
}
