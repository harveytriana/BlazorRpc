
using BlazorRpc;

using Grpc.Net.Client;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, RPC!");

var serviceUrl = "https://localhost:7158/"; // blazor server

// var serviceUrl = "https://localhost:7002/"; // blazor server alone
// var serviceUrl = "http://localhost:3008/"; // blazor server alone (Docker test)

Console.WriteLine(serviceUrl);
if(serviceUrl.Contains("7158")) {
    Console.WriteLine("Runs multiple projects in Visual Studio. Press any key when server is ready\n");
    Console.ReadKey();
}
// parameter
var legs = new LegsRequest {
    A = 1.0,
    B = 1.0
};

GrpcTest().Wait();
SignalRTest().Wait();

Console.ReadKey();

async Task GrpcTest()
{
    try {
        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var rf = new RF.RFClient(channel);

        var hypotenuse = await rf.HypotenuseAsync(legs);

        Console.WriteLine("From GRPC\tHypotenuse({0},{1}) = {2}", legs.A, legs.B, hypotenuse.Y);
    }
    catch(Exception exception) {
        Console.WriteLine("GrpcTest exception\n{0}\n", exception.Message);
    }
}

async Task SignalRTest()
{
    try {
        var _connection = new HubConnectionBuilder()
               .WithUrl(serviceUrl + "RpcFunctionsHub")
               .AddMessagePackProtocol() // binary protocol
               .Build();
        await _connection.StartAsync();

        var hypotenuse = await _connection.InvokeAsync<double>("Hypotenuse", legs);

        Console.WriteLine("From SignalR\tHypotenuse({0},{1}) = {2}", legs.A, legs.B, hypotenuse);
    }
    catch(Exception exception) {
        Console.WriteLine("SignalRTest exception\n{0}\n", exception.Message);
    }
}