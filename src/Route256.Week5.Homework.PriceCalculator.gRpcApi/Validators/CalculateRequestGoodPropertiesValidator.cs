using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Validators;

public class CalculateRequestGoodPropertiesValidator : AbstractValidator<GoodProperies>
{
    public CalculateRequestGoodPropertiesValidator()
    {
        RuleFor(x => x.Height)
            .GreaterThan(0);

        RuleFor(x => x.Length)
            .GreaterThan(0);

        RuleFor(x => x.Width)
            .GreaterThan(0);

        RuleFor(x => x.Weight)
            .GreaterThan(0);
    }
}