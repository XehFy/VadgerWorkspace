using VadgerWorkspace.Web;
using Telegram.Bot;
using Microsoft.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTelegramBotClient(builder.Configuration);
builder.Services.AddTelegramBotAdmin(builder.Configuration);
builder.Services.AddTelegramBotEmployee(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
