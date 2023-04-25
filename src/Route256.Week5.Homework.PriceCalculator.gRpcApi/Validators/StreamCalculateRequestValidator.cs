using FluentValidation;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Validators;

public class StreamCalculateRequestValidator : AbstractValidator<StreamCalculateRequest>
{
    public StreamCalculateRequestValidator()
    {
        RuleFor(x => x.Good)
            .SetValidator(new CalculateRequestGoodPropertiesValidator());
    }
}