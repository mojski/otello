using Otello.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var ServiceName = "Otello";

var ServiceVersion = "1.0.0";

builder.Services.AddOpenTelemetry(builder.Configuration);
//builder.AddSeqTelemetry(ServiceName, ServiceVersion);
builder.Logging.UseOpenTelemetry(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
