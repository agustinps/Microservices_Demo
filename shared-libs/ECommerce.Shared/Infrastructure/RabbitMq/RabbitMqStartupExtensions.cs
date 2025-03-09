using ECommerce.shared.Infrastructure.EventBus.Abstraction;
using ECommerce.Shared.Infrastructure.EventBus.Abstraction;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace ECommerce.shared.Infrastructure.RabbitMq;
public static class RabbitMqStartupExtensions
{
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.RabbitMqSectionName).Bind(rabbitMqOptions);
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(rabbitMqOptions));
        services.AddSingleton<RabbitMqTelemetry>();

        return services;
    }
    public static IServiceCollection AddRabbitMqEventPublisher(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, RabbitMqEventBus>();

        return services;
    }

    public static IServiceCollection AddRabbitMqSubscriberService(this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.Configure<EventBusOptions>(
            configuration.GetSection(EventBusOptions.EventBusSectionName));
        services.AddHostedService<RabbitMqHostedService>();

        return services;
    }

    public static OpenTelemetryBuilder AddOpenTelemetryMetrics(
        this OpenTelemetryBuilder openTelemetryBuilder,
        string serviceName,
        IServiceCollection services,
        Action<MeterProviderBuilder>? customMetrics = null)
    {
        services.AddSingleton(new MetricFactory(serviceName));
        return openTelemetryBuilder
            .WithMetrics(builder =>
            {
                builder
                    .AddConsoleExporter()
                    .AddAspNetCoreInstrumentation()
                    .AddMeter(serviceName)
                    .AddPrometheusExporter();
                customMetrics?.Invoke(builder);
            });
    }

    public static void UsePrometheusExporter(this WebApplication webApplication) =>
        webApplication.UseOpenTelemetryPrometheusScrapingEndpoint();
}
