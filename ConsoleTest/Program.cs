
using Grpc.Net.Client;
using BlazorRpc;

Console.WriteLine("Hello, Blazor GRPC!");

Thread.Sleep(300);

var serviceUrl = "https://localhost:7158/";

Console.WriteLine(serviceUrl);
Console.WriteLine("Pres any key when rever is ready");
Console.ReadLine();

try {
    using var channel = GrpcChannel.ForAddress(serviceUrl);

    var rf = new RF.RFClient(channel);

    var legs = new LegsRequest {
        A = 1.0,
        B = 1.0
    };
    var hypotenuse = rf.Hypotenuse(legs);

    Console.WriteLine("Hypotenus({0},{1}) = {2}", legs.A, legs.B, hypotenuse.Y);
}
catch(Exception exception) {
    Console.WriteLine("Exception:\n" + exception.Message);
}
Console.ReadKey();