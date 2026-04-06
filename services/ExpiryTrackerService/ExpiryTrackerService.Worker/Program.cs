using ExpiryTrackerService.Infrastructure;
using ExpiryTrackerService.Infrastructure.Persistence;
using ExpiryTrackerService.Worker.Workers;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<ExpiryCheckerWorker>();
builder.Services.AddHttpClient("NotificationService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:NotificationService"]!);
});
var host = builder.Build();

// Auto migrate
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider
                  .GetRequiredService<ExpiryTrackerDbContext>();
    db.Database.EnsureCreated();
}

await host.RunAsync();