using Otello.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry(builder.Configuration);
builder.Logging.UseOpenTelemetry(builder.Configuration);

var app = builder.Build();

app.MapGet("/api/test", (ILogger<Program> logger) =>
{
    var guid = Guid.NewGuid();
    logger.LogError("Test error {guid}", guid);

    return Results.Ok($"Test error {guid}");
});

app.Run();
