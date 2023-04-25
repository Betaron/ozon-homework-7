using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

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
}
