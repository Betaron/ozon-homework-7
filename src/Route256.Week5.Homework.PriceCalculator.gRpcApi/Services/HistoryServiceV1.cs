using Grpc.Core;
using MediatR;
using Route256.Week5.Homework.PriceCalculator.Bll.Commands;
using Route256.Week5.Homework.PriceCalculator.Bll.Queries;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Protos.V1;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

public class HistoryServiceV1 : History.HistoryBase
{
    private readonly IMediator _mediator;

    public HistoryServiceV1(
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

    public override async Task Get(GetRequest request, IServerStreamWriter<GetResponse> responseStream, ServerCallContext context)
    {
        var query = new GetCalculationHistoryQuery(
            request.UserId,
            request.Take,
            request.Skip);
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