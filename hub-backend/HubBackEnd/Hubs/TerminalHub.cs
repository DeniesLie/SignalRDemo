using Microsoft.AspNetCore.SignalR;
using HubBackEnd.Protocols;
using System.Text.Json;

class TerminalHub : Hub
{
    private readonly ILogger _logger;

    public TerminalHub(ILogger<TerminalHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Connected to hub with ConnectionId: " + Context.ConnectionId);
        Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnId", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task SendStdInAsync(string stdInRequestStr)
    {
        var stdInRequest = JsonSerializer.Deserialize<StdInRequest>(stdInRequestStr);
        if (stdInRequest != null){
            var toMachines = stdInRequest.ToDevices;
            await Clients.Clients(toMachines!).SendAsync("ReceiveStdIn", stdInRequest);
            _logger.LogInformation("Send stdin from user {0} to devices: {1}", stdInRequest.FromUser, stdInRequest.ToDevices);
        }
        _logger.LogError("Wrong stdInReques format");
    }

    public async Task SendStdOutAsync(string stdOutResponseStr)
    {
        var stdOutResponse = JsonSerializer.Deserialize<StdOutResponse>(stdOutResponseStr);
        if (stdOutResponse != null){
            var toUser = stdOutResponse.ToUser;
            await Clients.Client(toUser!).SendAsync("receiveStdOut", stdOutResponse);
            _logger.LogInformation("Send stdout from device {0} to user {1}", stdOutResponse.FromDevice, toUser);
        }
        _logger.LogError("Wrong stdOutResponse format");
    }
}