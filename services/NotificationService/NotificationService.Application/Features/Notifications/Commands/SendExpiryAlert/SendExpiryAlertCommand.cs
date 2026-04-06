using MediatR;

namespace NotificationService.Application.Features.Notifications.Commands.SendExpiryAlert;

public record SendExpiryAlertCommand(
    Guid    PharmacyId,
    Guid    BatchId,
    string  ProductName,
    string  BatchNumber,
    int     DaysToExpiry,
    string  PharmacyEmail,
    string? PharmacyPhone,
    string? DeviceToken
) : IRequest<string>;