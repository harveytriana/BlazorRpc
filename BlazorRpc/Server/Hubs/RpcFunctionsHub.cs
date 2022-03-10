using Microsoft.AspNetCore.SignalR;

namespace BlazorRpc.Server.Hubs;

public class RpcFunctionsHub : Hub
{
    public double Hypotenuse(LegsRequest legs)
    {
        return Math.Sqrt(legs.A * legs.A + legs.B * legs.B);
    }
}
