using System.Globalization;
using System.Text;
using Route256.Week5.Homework.PriceCalculator.gRpcClient.Commands;

internal class Program
{
    private static void Main()
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
                    deliveryCalculator.Calculate();
                    break;
                case "v1.clear":
                    historyV1.Clear();
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

        Console.WriteLine(sb);
    }
}