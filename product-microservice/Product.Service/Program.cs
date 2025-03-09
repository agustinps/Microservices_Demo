using Auth.Service.Authentication;
using ECommerce.shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using Product.Service.EndPoints;
using Product.Service.Infrastructure.Data.EntityFramework;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();

builder.Services.AddOpenTelemetryTracing("Product",
        builder.Configuration,
        (traceBuilder) => traceBuilder.WithSqlInstrumentation()
    );

Log.Logger = SerilogConfigStartupExtension.AddLogger("product");
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MigrateDatabase();

app.RegisterEndpoints();

app.UseHttpsRedirection();

app.UseJwtAuthentication();


await app.RunAsync();


