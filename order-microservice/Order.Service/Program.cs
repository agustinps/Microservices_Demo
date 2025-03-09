using ECommerce.shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using OpenTelemetry.Metrics;
using Order.Service.EndPoints;
using Order.Service.Infrastructure.Data;
using Order.Service.Infrastructure.Data.MongoDb;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddRabbitMqEventBus(builder.Configuration)
            .AddRabbitMqEventPublisher();

builder.Services.AddSingleton<IOrderStore, MongoOrderStore>();

builder.Services.AddOpenTelemetryTracing("Order", builder.Configuration, (traceBuilder) =>
        traceBuilder.WithSqlInstrumentation())
    .AddOpenTelemetryMetrics("Order", builder.Services, (metricBuilder) =>
        metricBuilder.AddView("products-per-order", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = [1, 2, 5, 10]
        }));

Log.Logger = SerilogConfigStartupExtension.AddLogger("product");
builder.Host.UseSerilog();

var app = builder.Build();

app.UsePrometheusExporter();

// Configure the HTTP request pipeline.
app.RegisterEndPoints();
app.UseHttpsRedirection();



await app.RunAsync();


