using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

public class DeliveryService : Delivery.DeliveryBase
{
    private readonly IMediator _mediator;

    public DeliveryService(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Response> Calculate(Request request, ServerCallContext context)
    {
        var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    x.Weight))
                .ToArray());
        var result = await _mediator.Send(command, context.CancellationToken);

        return new Response
        {
            CalculationId = result.CalculationId,
            Price = result.Price.ToDecimalValue()
        };
    }
}