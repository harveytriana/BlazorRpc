using Grpc.Core;

namespace BlazorRpc.Services;

public class RFService : RF.RFBase
{
    public override Task<FunctionReply> Hypotenuse(LegsRequest request, ServerCallContext context)
    {
        return Task.FromResult(new FunctionReply {
            Y = Math.Sqrt(request.A * request.A + request.B * request.B)
        });
    }
}
