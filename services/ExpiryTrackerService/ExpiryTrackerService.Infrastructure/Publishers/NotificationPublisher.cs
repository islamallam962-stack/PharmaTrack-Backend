using ExpiryTrackerService.Application.Common.Interfaces;
using ExpiryTrackerService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;


namespace ExpiryTrackerService.Infrastructure.Publishers;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IHttpClientFactory             _http;
    private readonly IConfiguration                 _config;
    private readonly ILogger<NotificationPublisher> _logger;

    public NotificationPublisher(
        IHttpClientFactory http,
        IConfiguration config,
        ILogger<NotificationPublisher> logger)
    {
        _http   = http;
        _config = config;
        _logger = logger;
    }

    public async Task PublishExpiryAlertAsync(ExpiryAlert alert, CancellationToken ct)
    {
        try
        {
            var client = _http.CreateClient("NotificationService");

            // في PublishExpiryAlertAsync:
            var payload = new
            {
                pharmacyId = alert.PharmacyId,
                batchId = alert.BatchId,
                productName = alert.ProductName,
                batchNumber = alert.BatchNumber,
                daysToExpiry = alert.DaysToExpiry,
                // TODO: Fetch real contact info from PharmacyService via HTTP
                pharmacyEmail = "noreply@pharmatrack.com",
                pharmacyPhone = (string?)null,
                deviceToken = (string?)null
            };

            var response = await client.PostAsJsonAsync(
                "/api/notifications/expiry-alert", payload, ct);

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning(
                    "Notification Service returned {Status}", response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call Notification Service.");
        }
    }
}