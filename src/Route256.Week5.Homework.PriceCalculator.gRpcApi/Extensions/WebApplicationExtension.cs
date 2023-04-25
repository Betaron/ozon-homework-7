using Route256.Week5.Homework.PriceCalculator.gRpcApi.Services;

namespace Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication? MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<DeliveryService>();
        app.MapGrpcService<HistoryService>();

        return app;
    }
}
