using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Common.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Features.Notifications.Commands.SendExpiryAlert;

public class SendExpiryAlertCommandHandler
    : IRequestHandler<SendExpiryAlertCommand, string>
{
    private readonly IEmailSender               _email;
    private readonly ISmsSender                 _sms;
    private readonly IPushSender                _push;
    private readonly IDashboardNotifier         _dashboard;
    private readonly INotificationLogRepository _logs;
    private readonly IUnitOfWork                _uow;
    private readonly ILogger<SendExpiryAlertCommandHandler> _logger;

    public SendExpiryAlertCommandHandler(
        IEmailSender email, ISmsSender sms, IPushSender push,
        IDashboardNotifier dashboard, INotificationLogRepository logs,
        IUnitOfWork uow, ILogger<SendExpiryAlertCommandHandler> logger)
    {
        _email     = email;
        _sms       = sms;
        _push      = push;
        _dashboard = dashboard;
        _logs      = logs;
        _uow       = uow;
        _logger    = logger;
    }

    public async Task<string> Handle(
        SendExpiryAlertCommand request, CancellationToken ct)
    {
        var title   = $"تنبيه انتهاء صلاحية — {request.ProductName}";
        var message = $"الدواء: {request.ProductName}\n" +
                      $"رقم الدفعة: {request.BatchNumber}\n" +
                      $"متبقي: {request.DaysToExpiry} يوم";

        var tasks = new List<Task>
        {
            SendChannelAsync(NotificationChannel.Dashboard, request.PharmacyId,
                () => _dashboard.SendAsync(request.PharmacyId, title, message, ct),
                title, message, ct),

            SendChannelAsync(NotificationChannel.Email, request.PharmacyId,
                () => _email.SendAsync(request.PharmacyEmail, title, message, ct),
                title, message, ct),
        };

        if (!string.IsNullOrEmpty(request.PharmacyPhone))
            tasks.Add(SendChannelAsync(NotificationChannel.Sms, request.PharmacyId,
                () => _sms.SendAsync(request.PharmacyPhone, message, ct),
                title, message, ct));

        if (!string.IsNullOrEmpty(request.DeviceToken))
            tasks.Add(SendChannelAsync(NotificationChannel.Push, request.PharmacyId,
                () => _push.SendAsync(request.DeviceToken, title, message, ct),
                title, message, ct));

        await Task.WhenAll(tasks);

        return "Notifications sent.";
    }

    private async Task SendChannelAsync(
        NotificationChannel channel, Guid recipientId,
        Func<Task> send, string title, string message, CancellationToken ct)
    {
        var log = NotificationLog.Create(
            recipientId, title, message, channel, referenceType: "ExpiryAlert");

        await _logs.AddAsync(log, ct);
        await _uow.SaveChangesAsync(ct);

        try
        {
            await send();
            log.MarkAsSent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send {Channel} notification.", channel);
            log.MarkAsFailed(ex.Message);
        }

        _logs.Update(log);
        await _uow.SaveChangesAsync(ct);
    }
}