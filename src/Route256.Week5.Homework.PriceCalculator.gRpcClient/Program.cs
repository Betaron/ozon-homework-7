using System.Globalization;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class Program
{
    private static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");

        var deliveryCalculator = new DeliveryCalculator();

        while (true)
        {
            Console.Write("Enter command: ");
            var command = Console.ReadLine();

            switch (command?.ToLower())
            {
                case "calculate":
                    deliveryCalculator.Calculate();
                    break;
                default:
                    break;
            }
        }
    }
}