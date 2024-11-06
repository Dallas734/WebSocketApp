// WebSocketApp.Core/Interfaces/IMyHttpListenerContext.cs
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketApp.Core.Interfaces
{
    public interface IMyHttpListenerContext
    {
        bool IsWebSocketRequest { get; }
        Task<IMyWebSocketContext> AcceptWebSocketAsync(string subProtocol);
        HttpListenerRequest Request { get; }
        HttpListenerResponse Response { get; }
    }
}
