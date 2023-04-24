using System.Text;
using Grpc.Core;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class HistoryService
{
    private readonly History.HistoryClient _historyClient;

    public HistoryService(History.HistoryClient historyClient)
    {
        _historyClient = historyClient;
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

        var result = _historyClient.Clear(request);

        Console.WriteLine("Clearing completed successfully");

        return Task.CompletedTask;
    }

    public async Task Get()
    {
        var request = new GetRequest();

        try
        {
            Console.Write("Enter User id: ");
            request.UserId = long.Parse(Console.ReadLine()!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        var call = _historyClient.Get(request);

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

        await responseTask;
    }
}
