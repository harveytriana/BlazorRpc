using Microsoft.AspNetCore.SignalR;

namespace BlazorRpc.Server.Hubs;

public class RpcFunctionsHub : Hub
{
    public FunctionReply Hypotenuse(LegsRequest legs)
    {
        return new FunctionReply {
            Y = Math.Sqrt(legs.A * legs.A + legs.B * legs.B)
        };
    }
}
