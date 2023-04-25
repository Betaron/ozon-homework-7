using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

namespace Route256.Week5.Homework.PriceCalculator.ProtoLib.Extensions;

public static class DecimalExtensions
{
    public static DecimalValue ToDecimalValue(this decimal value) =>
        DecimalValue.FromDecimal(value);
}
