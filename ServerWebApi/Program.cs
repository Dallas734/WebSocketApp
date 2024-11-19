using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // �������� ������������
});

app.Run("http://localhost:5001"); // ������ ������� �� ����� 5000
