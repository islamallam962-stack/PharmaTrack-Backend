using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Common.Interfaces;

namespace NotificationService.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public Task JoinPharmacyGroup(string pharmacyId)
        => Groups.AddToGroupAsync(Context.ConnectionId, $"pharmacy-{pharmacyId}");

    public Task LeavePharmacyGroup(string pharmacyId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"pharmacy-{pharmacyId}");
}

// الـ implementation للـ IDashboardHub — بيستخدم SignalR
public class DashboardHubService : IDashboardHub
{
    private readonly IHubContext<NotificationHub> _hub;

    public DashboardHubService(IHubContext<NotificationHub> hub) => _hub = hub;

    public Task SendToPharmacyAsync(
        Guid pharmacyId, string title, string message, CancellationToken ct)
        => _hub.Clients
               .Group($"pharmacy-{pharmacyId}")
               .SendAsync("ReceiveAlert", new { title, message }, ct);
}