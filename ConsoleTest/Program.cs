
using Grpc.Net.Client;
using BlazorRpc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, RPC!");

Thread.Sleep(300);

var serviceUrl = "https://localhost:7158/";

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

Console.WriteLine("Bye");
Console.ReadKey();

async Task GrpcTest()
{
    try {
        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var rf = new RF.RFClient(channel);

        var hypotenuse = await rf.HypotenuseAsync(legs);

        Console.WriteLine("From GRPC\tHypotenus({0},{1}) = {2}", legs.A, legs.B, hypotenuse.Y);
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

        Console.WriteLine("From SignalR\tHypotenus({0},{1}) = {2}", legs.A, legs.B, hypotenuse);
    }
    catch { }
}