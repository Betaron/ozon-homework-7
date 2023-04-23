using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;

public static class DecimalExtensions
{
    public static DecimalValue ToDecimalValue(this decimal value) =>
        DecimalValue.FromDecimal(value);
}
