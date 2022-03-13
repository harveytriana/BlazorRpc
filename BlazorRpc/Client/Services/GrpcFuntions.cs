using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;

namespace BlazorRpc.Client.Services;

public class GrpcFuntions : IAsyncDisposable
{
    readonly GrpcChannel? _channel;
    readonly RF.RFClient? _rfClient;

    public GrpcFuntions(NavigationManager navigation)
    {
        try {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var baseUri = navigation.BaseUri;
            _channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
            _rfClient = new RF.RFClient(_channel);
        }
        catch(Exception exception) {
            Console.WriteLine("Exception: {0}", exception.Message);
        }
    }

    public async Task<double> HypotenuseAsync(LegsRequest legs)
    {
        if(_rfClient is null) {
            return 0.0;
        }
        var reply = await _rfClient.HypotenuseAsync(legs);
        return reply.Y;
    }

    public async ValueTask DisposeAsync()
    {
        if(_channel is null) {
            return;
        }
        await _channel.ShutdownAsync(); // recommended
        GC.SuppressFinalize(this);
    }
}
