using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� ��� ������������ � Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // ���������� ��������� ��� API ������������
builder.Services.AddSwaggerGen(); // ��������� Swagger

var app = builder.Build();

// ��������� Swagger ��� ����������� ������������ API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ��������� ������������ Swagger
    app.UseSwaggerUI(); // ��������� ������������ ��� Swagger
}

app.UseRouting(); // ��������� ���������
var webSocketOptions = new Microsoft.AspNetCore.Builder.WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2), // �������� �������� ����������
    ReceiveBufferSize = 1024 * 4 // ������ ������ ������
};
app.UseWebSockets(webSocketOptions);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // �������� ������������
});

app.Run("http://localhost:5001"); // ������ ������� �� ����� 5000
