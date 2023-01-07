using VadgerWorkspace.Web;
using Telegram.Bot;
using Microsoft.AspNetCore;
using VadgerWorkspace.Domain.Services;
using VadgerWorkspace.Domain.Abstractions;
using VadgerWorkspace.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTelegramBotClient(builder.Configuration);
builder.Services.AddTelegramBotAdmin(builder.Configuration);
builder.Services.AddTelegramBotEmployee(builder.Configuration);

builder.Services.AddScoped<ICommandService, AdminCommandService>();
builder.Services.AddScoped<ICommandService, ClientCommandService>();
builder.Services.AddScoped<ICommandService, EmployeeCommandService>();

builder.Services.AddScoped<ICommandService, AdminNoCommandService>();
builder.Services.AddScoped<ICommandService, ClientNoCommandService>();
builder.Services.AddScoped<ICommandService, EmployeeNoCommandService>();

builder.Services.AddDbContext<VadgerContext>(opt
    => opt.UseSqlite($"Filename = C:/Users/Home/source/repos/VadgerWorkspace/VadgerDb.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
