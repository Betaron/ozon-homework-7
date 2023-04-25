using FluentValidation.AspNetCore;
using Route256.Week5.Homework.PriceCalculator.Bll.Extensions;
using Route256.Week5.Homework.PriceCalculator.Dal.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Extensions;
using Route256.Week5.Homework.PriceCalculator.gRpcApi.Interceptors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddGrpcReflection()
    .AddGrpc(o =>
    {
        o.Interceptors.Add<ExceptionInterceptor>();
    });

builder.Services
    .AddBll()
    .AddDalInfrastructure(builder.Configuration)
    .AddDalRepositories();

builder.Services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    conf.AutomaticValidationEnabled = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcReflectionService();
app.MapGrpcServices();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MigrateUp();
app.Run();
