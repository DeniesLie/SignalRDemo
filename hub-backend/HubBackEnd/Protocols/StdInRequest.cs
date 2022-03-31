namespace HubBackEnd.Protocols;

public class StdInRequest
{
    public string? FromUser { get; set; }
    public string[]? ToDevices { get; set; }
    public string? StdIn { get; set; }
}