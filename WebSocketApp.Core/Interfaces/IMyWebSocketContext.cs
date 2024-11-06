using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketApp.Core.Interfaces
{
    public interface IMyWebSocketContext
    {
        IMyWebSocket WebSocket { get; }
    }
}
