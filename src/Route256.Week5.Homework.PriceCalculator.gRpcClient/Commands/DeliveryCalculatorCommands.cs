using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Interceptors;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class DeliveryCalculatorCommands
{
    private readonly IHost _host;
    public DeliveryCalculatorCommands()
    {
        _host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddLogging(o => o.AddConsole())
                    .AddSingleton<LoggingInterceptor>()
                    .AddGrpcClient<Delivery.DeliveryClient>(o =>
                    {
                        o.Address = new Uri("http://localhost:5141");
                    })
                    .AddInterceptor<LoggingInterceptor>();
            })
            .Build();
    }

    public Task Calculate()
    {
        var request = new Request();

        try
        {
            Console.Write("Enter User id: ");
            request.UserId = long.Parse(Console.ReadLine()!);
            Console.Write("Enter number of goods: ");
            var goodsNumber = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter product properties separated by commas ('height,length,width,weight'): ");
            for (int i = 0; i < goodsNumber; i++)
            {
                var propertiesString = Console.ReadLine();
                var properties = propertiesString?.Split(',').Select(x => x.Trim()).ToArray();
                request.Goods.Add(new GoodProperies()
                {
                    Height = double.Parse(properties[0]),
                    Length = double.Parse(properties[1]),
                    Width = double.Parse(properties[2]),
                    Weight = double.Parse(properties[3])
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Task.FromException(ex);
        }

        _host.StartAsync();

        var client = _host.Services.GetRequiredService<Delivery.DeliveryClient>();
        var result = client.Calculate(request);

        _host.StopAsync();

        Console.WriteLine(
            $"Calculation id: {result.CalculationId}\n" +
            $"Price: {result.Price.ToDecimal()}");

        return Task.CompletedTask;
    }
}
