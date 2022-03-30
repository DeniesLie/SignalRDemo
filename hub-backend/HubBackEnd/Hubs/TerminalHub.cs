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
        var toMachines = stdInRequest.ToDevices;
        await Clients.Clients(toMachines).SendAsync("ReceiveStdIn", stdInRequest);
        _logger.LogInformation("Send stdin from user {0} to devices: {1}", stdInRequest.FromUser, stdInRequest.ToDevices);
    }

    public async Task SendStdOut(string stdOutRequestStr)
    {
        var stdOutRequest = JsonSerializer.Deserialize<StdOutRequest>(stdOutRequestStr);
        var toUser = stdOutRequest.ToUser;
        await Clients.Client(toUser).SendAsync("ReceiveStdIn", stdOutRequest);
        _logger.LogInformation("Send stdout from device {0} to user {1}", stdOutRequest.FromDevice, toUser);
    }
}