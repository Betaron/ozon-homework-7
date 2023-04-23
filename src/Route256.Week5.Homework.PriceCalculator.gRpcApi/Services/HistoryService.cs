using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

public class HistoryService : History.HistoryBase
{
    private readonly IMediator _mediator;

    public HistoryService(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Empty> Clear(ClearRequest request, ServerCallContext context)
    {
        var query = new ClearHistoryCommand(
            request.UserId,
            request.CalculationIds.ToArray()
        );

        await _mediator.Send(query, context.CancellationToken);

        return new Empty();
    }
}