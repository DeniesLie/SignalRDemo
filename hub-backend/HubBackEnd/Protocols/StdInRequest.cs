namespace HubBackEnd.Protocols;

public struct StdInRequest
{
    public string FromUser;
    public string[] ToDevices;
    public string StdIn;
}