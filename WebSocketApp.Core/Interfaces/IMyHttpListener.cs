// WebSocketApp.Core/Interfaces/IMyHttpListener.cs
using System.Net;
using System.Threading.Tasks;

namespace WebSocketApp.Core.Interfaces
{
    public interface IMyHttpListener
    {
        void Start();
        void Stop();
        Task<HttpListenerContext> GetContextAsync();

        // Добавляем метод AddPrefix
        void AddPrefix(string prefix);
    }
}
