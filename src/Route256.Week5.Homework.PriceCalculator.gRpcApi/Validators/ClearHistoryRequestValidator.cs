using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Validators;

public class ClearHistoryRequestValidator : AbstractValidator<ClearRequest>
{
    public ClearHistoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}