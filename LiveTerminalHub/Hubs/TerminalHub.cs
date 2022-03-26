using Microsoft.AspNetCore.SignalR;

namespace LiveTerminalHub.Hubs;

public class TerminalHub : Hub
{
    private readonly ILogger _logger;

    public TerminalHub(ILogger logger)
    {
        _logger = logger;
    }
    
    // send from device to client
    public async Task SendStdOut(string user, string stdOut)
        => await Clients.User(user).SendAsync("receiveStdOut", stdOut);
    
    // send from client to device
    public async Task SendStdIn(IEnumerable<string> devices, string stdIn)
        => await Clients.Users(devices).SendAsync("ReceiveStdIn", stdIn);
    
    // send from client to multiple devices
    //public async Task SendStdInToMany(string stdIn) 

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation(
            $"{Context.UserIdentifier ?? "Unknown user"} is connected");
    }
}