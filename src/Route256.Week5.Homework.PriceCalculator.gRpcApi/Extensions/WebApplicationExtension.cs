using Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication? MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<DeliveryServiceV1>();
        app.MapGrpcService<HistoryServiceV1>();
        app.MapGrpcService<HistoryServiceV2>();

        return app;
    }
}
