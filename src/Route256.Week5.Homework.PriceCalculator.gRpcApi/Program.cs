using Route256.Week5.Homework.PriceCalculator.Bll.Extensions;
using Route256.Week5.Homework.PriceCalculator.Dal.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services
            .AddGrpcReflection()
            .AddGrpc();

        builder.Services
            .AddBll()
            .AddDalInfrastructure(builder.Configuration)
            .AddDalRepositories();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcReflectionService();
        app.MapGrpcServices();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        app.MigrateUp();
        app.Run();
    }
}