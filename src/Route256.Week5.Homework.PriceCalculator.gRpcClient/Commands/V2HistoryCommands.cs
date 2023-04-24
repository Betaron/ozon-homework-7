using System.Text;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Interceptors;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos.V2;

namespace Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class V2HistoryCommands
{
    private readonly IHost _host;
    public V2HistoryCommands()
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

    public async Task Get()
    {
        var request = new GetRequest();

        try
        {
            Console.Write("Enter User id: ");
            request.UserId = long.Parse(Console.ReadLine()!);
            Console.Write("Enter Take: ");
            request.Take = int.Parse(Console.ReadLine()!);
            Console.Write("Enter Skip: ");
            request.Skip = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter calculation ids separated by commas ('1,2,3'): ");
            var idsString = Console.ReadLine();
            var ids = idsString?.Split(',').Select(x => long.Parse(x.Trim()));
            request.CalculationIds.Add(ids);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        await _host.StartAsync();

        var client = _host.Services.GetRequiredService<History.HistoryClient>();
        var call = client.Get(request);

        var responseTask = Task.Run(async () =>
        {
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                var ids = new StringBuilder();
                foreach (var id in response.Cargo.GoodIds)
                {
                    ids.Append($"{id}, ");
                }
                Console.WriteLine(
                    $"Cargo:\n" +
                    $"\tGoods ids: {ids}\n" +
                    $"\tVolume: {response.Cargo.Volume}\n" +
                    $"\tWeight: {response.Cargo.Weight}\n" +
                    $"Price: {response.Price.ToDecimal()}\n" +
                    $"__________________");
            }
        });

        await _host.StopAsync();

        await responseTask;
    }
}
