using Scalar.AspNetCore;
using WhisperServer.Api.DatabaseContext;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Hubs;
using WhisperServer.Api.Security;
using WhisperServer.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddDatabase(configuration)
    .AddSecurity(configuration)
    .AddSingleton<ExceptionMiddleware>()
    .AddServices()
    .AddHttpContextAccessor()
    .AddOpenApi()
    .AddControllers();

builder.Services.AddSignalR();


var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.ApplyMigrations();
}

if (app.Environment.IsDevelopment())
{
    app.UseSecurityDev();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (app.Environment.IsProduction())
{
    app.UseSecurityProd();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.MapHub<ChatHub>("/chat");
app.Run();

namespace WhisperServer.Api
{
    public partial class Program { }
}