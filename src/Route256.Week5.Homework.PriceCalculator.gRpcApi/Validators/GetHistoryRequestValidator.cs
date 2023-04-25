using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Validators;

public class GetHistoryRequestValidator : AbstractValidator<GetRequest>
{
    public GetHistoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}