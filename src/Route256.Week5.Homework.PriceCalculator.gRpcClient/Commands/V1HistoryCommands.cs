using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Interceptors;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class V1HistoryCommands
{
    private readonly IHost _host;
    public V1HistoryCommands()
    {
        _host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddLogging(o => o.AddConsole())
                    .AddSingleton<LoggingInterceptor>()
                    .AddGrpcClient<History.HistoryClient>(o =>
                    {
                        o.Address = new Uri("http://localhost:5141");
                    })
                    .AddInterceptor<LoggingInterceptor>();
            })
            .Build();
    }

    public Task Clear()
    {
        var request = new ClearRequest();

        try
        {
            Console.Write("Enter User id: ");
            request.UserId = long.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter calculation ids separated by commas ('1,2,3'): ");
            var idsString = Console.ReadLine();
            var ids = idsString?.Split(',').Select(x => long.Parse(x.Trim()));
            request.CalculationIds.Add(ids);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Task.FromException(ex);
        }

        _host.StartAsync();

        var client = _host.Services.GetRequiredService<History.HistoryClient>();
        var result = client.Clear(request);

        _host.StopAsync();

        Console.WriteLine("Clearing completed successfully");

        return Task.CompletedTask;
    }
}
