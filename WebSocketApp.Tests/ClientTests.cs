// WebSocketApp.Tests/ClientTests.cs
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using WebSocketApp.Client;
using WebSocketApp.Core.Interfaces;
using WebSocketApp.Server;
using Xunit;

namespace WebSocketApp.Tests
{
    public class WebSocketClientTests
    {
        [Fact]
        public async Task ConnectAsync_ShouldConnectToServer()
        {
            // Arrange
           /* var expectedUri = new Uri($"ws://localhost/8080");
            var webSocketMock = new Mock<IMyWebSocket>();
            var client = new WebSocketClient(webSocketMock.Object, "localhost", 8080);

            webSocketMock.Setup(ws => ws.ConnectAsync(expectedUri, It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask)
                      .Callback(() => webSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Open));

            // Act
            await client.ConnectAsync();

            // Assert
            webSocketMock.Verify(ws => ws.ConnectAsync(expectedUri, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(WebSocketState.Open, webSocketMock.Object.State);*/
        }
    }
}
