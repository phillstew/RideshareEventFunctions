using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RideshareEventFunctions.Configs;
using RideshareEventFunctions.Services;
using RideshareEventFunctions.Services.Interfaces;
using System.Configuration;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                // Add Dependencies
                services.AddTransient<IRideService, RideService>();
                services.AddSingleton<IRideShareEventProducer, RideShareEventProducer>();
            })
            .Build();

        await host.RunAsync();
    }
}