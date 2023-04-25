using FluentValidation;
using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;
using Route256.Week5.Homework.PriceCalculator.ProtoLib.Extensions;
using Route256.Week5.Homework.PriceCalculator.ProtoLib.Protos;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

public class HistoryService : History.HistoryBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<GetRequest> _getValidator;
    private readonly IValidator<ClearRequest> _clearValidator;

    public HistoryService(
        IMediator mediator,
        IValidator<GetRequest> getLogger,
        IValidator<ClearRequest> clearValidator)
    {
        _mediator = mediator;
        _getValidator = getLogger;
        _clearValidator = clearValidator;
    }

    public override async Task<Empty> Clear(ClearRequest request, ServerCallContext context)
    {
        var validationResult = await _clearValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var query = new ClearHistoryCommand(
            request.UserId,
            request.CalculationIds.ToArray()
        );

        await _mediator.Send(query, context.CancellationToken);

        return new Empty();
    }

    public override async Task Get(GetRequest request, IServerStreamWriter<GetResponse> responseStream, ServerCallContext context)
    {
        var validationResult = await _getValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var query = new GetCalculationHistoryQuery(
            request.UserId,
            int.MaxValue,
            0);
        var result = await _mediator.Send(query, context.CancellationToken);

        foreach (var item in result.Items)
        {
            var response = new GetResponse
            {
                Cargo = new CargoResponse
                {
                    Volume = item.Volume,
                    Weight = item.Weight
                },
                Price = item.Price.ToDecimalValue()
            };
            response.Cargo.GoodIds.Add(item.GoodIds);

            await responseStream.WriteAsync(response);
        };
    }
}