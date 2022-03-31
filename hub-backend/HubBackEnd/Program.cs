var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure services
builder.Services.AddCors();
builder.Services.AddSignalR();


var app = builder.Build();
// Configure middleware pipeline
app.UseCors(builder => builder
    .WithOrigins("null", "localhost")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);
app.MapHub<TerminalHub>("/terminal");
app.MapGet("/test", () => "It works!");


app.Run("https://localhost:5000");
