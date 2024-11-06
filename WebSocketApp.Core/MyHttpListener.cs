// WebSocketApp.Core/MyHttpListener.cs
using System.Net;
using System.Threading.Tasks;
using WebSocketApp.Core.Interfaces;

namespace WebSocketApp.Core
{
    public class MyHttpListener : IMyHttpListener
    {
        private readonly HttpListener _httpListener = new HttpListener();

        public void Start()
        {
            _httpListener.Start();
        }

        public void Stop()
        {
            _httpListener.Stop();
        }

        public async Task<HttpListenerContext> GetContextAsync()
        {
            return await _httpListener.GetContextAsync();
        }

        // Реализация метода AddPrefix
        public void AddPrefix(string prefix)
        {
            _httpListener.Prefixes.Add(prefix);
        }
    }
}
