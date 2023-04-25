using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.Bll.Services.Interfaces;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Commands;

public record CalculateDeliveryPriceWithoutSavingCommand(GoodModel Good)
    : IRequest<decimal>;

public class CalculateDeliveryPriceWithoutSavingCommandHandler
    : IRequestHandler<CalculateDeliveryPriceWithoutSavingCommand, decimal>
{
    private readonly ICalculationService _calculationService;

    public CalculateDeliveryPriceWithoutSavingCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    public Task<decimal> Handle(
        CalculateDeliveryPriceWithoutSavingCommand request,
        CancellationToken cancellationToken)
    {
        var volumePrice = _calculationService.CalculatePriceByVolume(new[] { request.Good }, out var _);
        var weightPrice = _calculationService.CalculatePriceByWeight(new[] { request.Good }, out var _);
        var resultPrice = Math.Max(volumePrice, weightPrice);

        return Task.FromResult(resultPrice);
    }
}