using System.Globalization;
using System.Text;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class Program
{
    private static async Task Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");

        PrintHelp();

        var deliveryCalculator = new DeliveryCalculatorCommands();
        var historyV1 = new V1HistoryCommands();

        while (true)
        {
            Console.Write("Enter command: ");
            var command = Console.ReadLine();

            switch (command?.ToLower())
            {
                case "calculate":
                    await deliveryCalculator.Calculate();
                    break;
                case "v1.clear":
                    await historyV1.Clear();
                    break;
                case "v1.get":
                    await historyV1.Get();
                    break;
                case null:
                    return;
                default:
                    break;
            }
        }
    }

    static void PrintHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available commands:");
        sb.AppendLine("\t- calcuate");
        sb.AppendLine("\t- v1.clear");
        sb.AppendLine("\t- v1.get");

        Console.WriteLine(sb);
    }
}