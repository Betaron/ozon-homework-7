using Grpc.Core;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class DeliveryCalculatorService
{
    private readonly Delivery.DeliveryClient _deliveryClient;

    public DeliveryCalculatorService(Delivery.DeliveryClient deliveryClient)
    {
        _deliveryClient = deliveryClient;
    }

    public Task Calculate()
    {
        var request = new CalculateRequest();

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

        var result = _deliveryClient.Calculate(request);

        Console.WriteLine(
            $"Calculation id: {result.CalculationId}\n" +
            $"Price: {result.Price.ToDecimal()}");

        return Task.CompletedTask;
    }

    public async Task StreamCalculate()
    {
        var call = _deliveryClient.StreamCalculate();

        var responseTask = Task.Run(async () =>
        {
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Price: {response.Price.ToDecimal()}");
            }
        });

        Console.WriteLine("Enter file name: ");
        var path = Console.ReadLine();
        if (!File.Exists(path ?? string.Empty))
        {
            Console.WriteLine("The file does not exist");
            return;
        }

        using (var file = File.OpenText(path))
        {
            while (!file.EndOfStream)
            {
                var request = new StreamCalculateRequest();

                try
                {
                    var propertiesString = file.ReadLine();
                    var properties = propertiesString?.Split(',').Select(x => x.Trim()).ToArray();
                    request.Good = new GoodProperies()
                    {
                        Height = double.Parse(properties[0]),
                        Length = double.Parse(properties[1]),
                        Width = double.Parse(properties[2]),
                        Weight = double.Parse(properties[3])
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }

                await call.RequestStream.WriteAsync(request);
            }

            await call.RequestStream.CompleteAsync();
            await responseTask;
        }
    }
}
