// WebSocketApp.Tests/ServerTests.cs
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using WebSocketApp.Core;
using WebSocketApp.Core.Interfaces;
using WebSocketApp.Server;
using Xunit;

namespace WebSocketApp.Tests
{
    public class WebSocketServerTests
    {
        [Fact]
        public async Task Start_ShouldStartWebSocketConnection()
        {
            // Arrange
            var mockHttpListener = new Mock<IMyHttpListener>();

            var server = new WebSocketServer(mockHttpListener.Object, "localhost", 8080);

            var serverTask = server.Start();

            mockHttpListener.Verify(x => x.Start(), Times.Once);

            // Даем серверу время для запуска и обработки тестового подключения
            await Task.Delay(1000);

        }

        [Fact]
        public async Task Start_ShouldStopWebSocketConnection()
        {
            // Arrange
            var mockHttpListener = new Mock<IMyHttpListener>();

            var server = new WebSocketServer(mockHttpListener.Object, "localhost", 8080);

            var serverTask = server.Start();

            await server.Stop();

            mockHttpListener.Verify(x => x.Stop(), Times.Once);

            // Даем серверу время для запуска и обработки тестового подключения
            await Task.Delay(1000);
        }

        [Fact]
        public async Task RecieveMessage()
        {
            var buffer = Encoding.UTF8.GetBytes("sad");
            var httpContextMock = new Mock<IMyHttpListenerContext>();
            var webSocketContextMock = new Mock<IMyWebSocketContext>();
            var webSocketMock = new Mock<IMyWebSocket>();
            var receiveResult = new WebSocketReceiveResult(buffer.Length, WebSocketMessageType.Text, true);
            var httpListenerMock = new Mock<IMyHttpListener>();
            var server = new WebSocketServer(httpListenerMock.Object, "localhost", 8080);

            var serverResponse = "Сообщение принято: Hello Server!";

            // Настраиваем поведение запроса
            httpContextMock.Setup(c => c.Request.IsWebSocketRequest).Returns(true);
            httpContextMock.Setup(c => c.AcceptWebSocketAsync(null)).ReturnsAsync(webSocketContextMock.Object);
            webSocketContextMock.Setup(w => w.WebSocket).Returns(webSocketMock.Object);

            // Настраиваем поведение WebSocket для получения сообщения
            webSocketMock.SetupSequence(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(receiveResult) // Имитация получения сообщения
                         .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true)); // Закрытие соединения

            webSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Open);

            // Настраиваем поведение WebSocket для отправки ответа
            webSocketMock.Setup(ws => ws.SendAsync(It.Is<ArraySegment<byte>>(b => Encoding.UTF8.GetString(b.Array, b.Offset, b.Count) == serverResponse),
                                                   WebSocketMessageType.Text,
                                                   true,
                                                   It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // httpListenerMock.Setup(x => x.GetContextAsync()).ReturnsAsync(httpContextMock.Object);

            // Act: Запускаем сервер
            var startTask = server.Start();

            // Даем серверу время на выполнение
            await Task.Delay(100);

            // Assert
            webSocketMock.Verify(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()), Times.Once);
            webSocketMock.Verify(ws => ws.SendAsync(It.Is<ArraySegment<byte>>(b => Encoding.UTF8.GetString(b.Array, b.Offset, b.Count) == serverResponse),
                                                    WebSocketMessageType.Text,
                                                    true,
                                                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
