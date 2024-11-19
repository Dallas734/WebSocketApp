using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов для контроллеров и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Добавление поддержки для API документации
builder.Services.AddSwaggerGen(); // Настройка Swagger

var app = builder.Build();

// Включение Swagger для отображения документации API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Генерация спецификации Swagger
    app.UseSwaggerUI(); // Интерфейс пользователя для Swagger
}

// Маршруты и обработка HTTP запросов
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Привязка контроллеров
});

app.Run("http://localhost:5000"); // Запуск клиента на порту 5000
