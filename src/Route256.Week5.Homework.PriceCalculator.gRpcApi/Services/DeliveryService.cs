using FluentValidation;
using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Models;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

public class DeliveryService : Delivery.DeliveryBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CalculateRequest> _calculateValidator;
    private readonly IValidator<StreamCalculateRequest> _streamCalculateValidator;

    public DeliveryService(
        IMediator mediator,
        IValidator<CalculateRequest> calculateValidator,
        IValidator<StreamCalculateRequest> streamCalculateValidator)
    {
        _mediator = mediator;
        _calculateValidator = calculateValidator;
        _streamCalculateValidator = streamCalculateValidator;
    }

    public override async Task<CalculateResponse> Calculate(
        CalculateRequest request, ServerCallContext context)
    {
        var validationResult = await _calculateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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

        return new CalculateResponse
        {
            CalculationId = result.CalculationId,
            Price = result.Price.ToDecimalValue()
        };
    }

    public override async Task StreamCalculate(
        IAsyncStreamReader<StreamCalculateRequest> requestStream,
        IServerStreamWriter<CalculateResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var validationResult = await _streamCalculateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var command = new CalculateDeliveryPriceCommand(
            request.UserId,
            new GoodModel[]
            {
                new(
                    request.Good.Height,
                    request.Good.Length,
                    request.Good.Width,
                    request.Good.Weight)
            });

            var result = await _mediator.Send(command, context.CancellationToken);

            await responseStream.WriteAsync(new CalculateResponse
            {
                CalculationId = result.CalculationId,
                Price = result.Price.ToDecimalValue()
            });
        }
    }
}