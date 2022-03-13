
using BlazorRpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, RPC!");

var serviceUrl = "https://localhost:7158/"; // blazor server

Console.WriteLine(serviceUrl);
Console.WriteLine("Press any key when server is ready\n");
Console.ReadKey();

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
    catch { }
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
    catch { }
}