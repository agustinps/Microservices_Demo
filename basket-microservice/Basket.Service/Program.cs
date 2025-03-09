using Basket.Service.EndPoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.Data.Redis;
using Basket.Service.IntegrationEvents;
using Basket.Service.IntegrationEvents.EventHandlers;
using ECommerce.shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqSubscriberService(builder.Configuration)
    .AddEventHandler<OrderCreatedEvent, OrderCreatedEventHandler>()
    .AddEventHandler<ProductPriceUpdatedEvent, ProductPriceUpdatedEventHandler>();

builder.Services.AddHostedService<RabbitMqHostedService>();
builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddScoped<IBasketStore, RedisBasketStore>();
builder.Services
    .AddOpenTelemetryTracing("Basket", builder.Configuration);

Log.Logger = SerilogConfigStartupExtension.AddLogger("product");
builder.Host.UseSerilog();

var app = builder.Build();

app.RegisterEndPoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



await app.RunAsync();

