using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Common.Interfaces;
using NotificationService.Application.DTOs;
using NotificationService.Application.Features.Notifications.Commands.SendExpiryAlert;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator                  _mediator;
    private readonly INotificationLogRepository _logs;

    public NotificationsController(
        IMediator mediator, INotificationLogRepository logs)
    {
        _mediator = mediator;
        _logs     = logs;
    }

    // بيُستدعى داخلياً من الـ Expiry Tracker Service
    [HttpPost("expiry-alert")]
    public async Task<IActionResult> SendExpiryAlert(
        SendExpiryAlertCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, message = result });
    }

    // الصيدلي يشوف الـ notifications بتاعته
    [HttpGet("{pharmacyId:guid}")]
    public async Task<IActionResult> GetMyNotifications(
        Guid pharmacyId, CancellationToken ct)
    {
        var logs = await _logs.GetByRecipientAsync(pharmacyId, ct);
        var dtos = logs.Select(n => new NotificationLogDto(
            n.Id, n.Title, n.Message,
            n.Channel.ToString(), n.Status.ToString(), n.CreatedAt
        )).ToList();

        return Ok(new { success = true, data = dtos });
    }
}