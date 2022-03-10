using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorRpc.Client.Services
{
    public class SrFunctions : IAsyncDisposable
    {
        HubConnection? _connection;

        public SrFunctions(NavigationManager navigation)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(navigation.ToAbsoluteUri("/RpcFunctionsHub"))
                .AddMessagePackProtocol()
                .Build();

            Task.Run(async () => {
                try {
                    await _connection.StartAsync();
                }
                catch(Exception exception) {
                    Console.WriteLine("Exception: {0}", exception.Message);
                    _connection = null;
                }
            });
        }

        public async Task<double> HypotenuseAsync(LegsRequest legs)
        {
            if(_connection is not null) {
                return await _connection.InvokeAsync<double>("Hypotenuse", legs);
            }
            return 0.0;
        }

        public async ValueTask DisposeAsync()
        {
            if(_connection is not null) {
                await _connection.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
}
