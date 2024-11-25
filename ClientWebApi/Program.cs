using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

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

// �������� � ��������� HTTP ��������
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // �������� ������������
});

app.Run("http://localhost:5000"); // ������ ������� �� ����� 5000
