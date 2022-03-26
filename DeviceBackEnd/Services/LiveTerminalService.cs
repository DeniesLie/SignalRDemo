using Microsoft.AspNetCore.SignalR.Client;

namespace DeviceBackEnd.Services;

public class LiveTerminalService
{
    private readonly HubConnection _connection;
    private readonly ILogger _logger;
    
    public LiveTerminalService(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;
        
        _connection = new HubConnectionBuilder()
            .WithUrl(configuration["MasterServer:IpAddress"])
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string>("SendStdIn", (stdIn) =>
        {
            _logger.LogInformation($"Message '{stdIn}' sent at {DateTime.Now}");
        });
    }

    public async Task StartAsync()
    {
        try
        {
            await _connection.StartAsync();
            _logger.LogInformation($"Connection is started");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Connection failed: {ex}");
        }
    }

    public async Task SendStdOut(string stdOut)
    {
        try
        {
            await _connection.InvokeAsync("SendStdOut", stdOut);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send stdOut: {ex}");
        }
    }
}