using System.Globalization;
using System.Text;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos;


var clientOptions = new Action<GrpcClientFactoryOptions>(o =>
        {
            o.Address = new Uri("http://localhost:5141");
        });

var hostBuilder = new HostBuilder();

hostBuilder.ConfigureServices(services =>
{
    services.AddLogging(o => o.AddConsole());

    services.AddGrpcClient<Delivery.DeliveryClient>(clientOptions);
    services.AddGrpcClient<History.HistoryClient>(clientOptions);

    services.AddSingleton<DeliveryCalculatorService>();
    services.AddSingleton<HistoryService>();
});

var host = hostBuilder.Build();

host.Start();

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");

PrintHelp();

var deliveryCalculator = host.Services.GetRequiredService<DeliveryCalculatorService>();
var history = host.Services.GetRequiredService<HistoryService>();

while (true)
{
    Console.Write("Enter command: ");
    var command = Console.ReadLine();

    switch (command?.ToLower())
    {
        case "calculate":
            await deliveryCalculator.Calculate();
            break;
        case "clear":
            await history.Clear();
            break;
        case "get":
            await history.Get();
            break;
        case null:
            return;
        default:
            break;
    }
}

static void PrintHelp()
{
    var sb = new StringBuilder();
    sb.AppendLine("Available commands:");
    sb.AppendLine("\t- calcuate");
    sb.AppendLine("\t- clear");
    sb.AppendLine("\t- get");
    sb.AppendLine("\nCtrl + C to exit.");

    Console.WriteLine(sb);
}