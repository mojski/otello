namespace Otello.WebApi.Extensions;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(OpenTelemetryOptions.SECTION_NAME).Get<OpenTelemetryOptions>();

        options ??= new OpenTelemetryOptions();

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Otello", serviceVersion: "1.0"))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(otlpConfig =>
                {
                    if (string.IsNullOrWhiteSpace(options.Authorisation) is false)
                    {
                        otlpConfig.Headers = $"{options.AuthorisationHeader}={options.Authorisation}";
                    }

                    otlpConfig.Endpoint = new Uri(options.Endpoint);
                    otlpConfig.Protocol = options.Protocol;
                }))
            ;
    }

    public static void AddOpenTelemetryCollectorx(this WebApplicationBuilder builder, string serviceName, string serviceVersion)
    {
        builder.Logging.AddOpenTelemetry(cfg =>
        {
            cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
                .AddOtlpExporter(otlpConfig =>
                {
                    otlpConfig.Endpoint = new Uri("http://0.0.0.0:4317");
                    //otlpConfig.Headers = "X-Seq-ApiKey=ZmnK7mpNojpgl2m6uG8V";
                    otlpConfig.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                });
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName, serviceVersion: serviceVersion))
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(cfg =>
                {
                    cfg.Endpoint = new Uri("http://0.0.0.0:4317");
                    //cfg.Headers = "X-Seq-ApiKey=ZmnK7mpNojpgl2m6uG8V";
                    cfg.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                }))
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                .AddOtlpExporter());
    }

    public static void AddSeqTelemetry(this WebApplicationBuilder builder, string serviceName, string serviceVersion)
    {
        builder.Logging.AddOpenTelemetry(cfg =>
        {
            cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion)).AddOtlpExporter(otlpConfig =>
            {
                otlpConfig.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
                otlpConfig.Headers = "X-Seq-ApiKey=ZmnK7mpNojpgl2m6uG8V";
                otlpConfig.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            });
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName, serviceVersion: serviceVersion))
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(cfg =>
                {
                    cfg.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
                    cfg.Headers = "X-Seq-ApiKey=ZmnK7mpNojpgl2m6uG8V";
                    cfg.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                }))
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation().AddOtlpExporter());
    }

    public static void UseOpenTelemetry(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var options = configuration.GetSection(OpenTelemetryOptions.SECTION_NAME).Get<OpenTelemetryOptions>();

        options ??= new OpenTelemetryOptions();

        loggingBuilder.AddOpenTelemetry(cfg =>
        {
            cfg.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Otello", serviceVersion: "1.0"))
                .AddOtlpExporter(otlpConfig =>
                {
                    if (string.IsNullOrWhiteSpace(options.Authorisation) is false)
                    {
                        otlpConfig.Headers = $"{options.AuthorisationHeader}={options.Authorisation}";
                    }

                    otlpConfig.Endpoint = new Uri(options.Endpoint);
                    otlpConfig.Protocol = options.Protocol;
                })
                ;
        });
    }
}
