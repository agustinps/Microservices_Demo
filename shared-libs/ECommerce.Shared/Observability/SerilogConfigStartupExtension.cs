using Serilog;
using Serilog.Core;

namespace ECommerce.Shared.Observability;
public static class SerilogConfigStartupExtension
{

    public static Logger AddLogger(string serviceName)
    {
        const string outputTemplate =
             "[{Level:w}]: {Timestamp:dd-MM-yyyy:HH:mm:ss} {MachineName} {EnvironmentName} {SourceContext} {Message}{NewLine}{Exception}";

        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate)
            .WriteTo.OpenTelemetry(opts =>
            {
                opts.ResourceAttributes = new Dictionary<string, object>
                {
                    ["app"] = "WebApi",
                    ["runtime"] = "dotnet",
                    ["service.name"] = serviceName
                };
            })
            .CreateLogger();
    }
}
