using NotificationService.Domain.Common;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Entities;

public class NotificationLog : BaseEntity
{
    public Guid                RecipientId   { get; private set; }
    public string              Title         { get; private set; } = default!;
    public string              Message       { get; private set; } = default!;
    public NotificationChannel Channel       { get; private set; }
    public NotificationStatus  Status        { get; private set; } = NotificationStatus.Pending;
    public string?             ErrorMessage  { get; private set; }
    public Guid?               ReferenceId   { get; private set; }
    public string?             ReferenceType { get; private set; }

    private NotificationLog() { }

    public static NotificationLog Create(
        Guid recipientId,
        string title,
        string message,
        NotificationChannel channel,
        Guid? referenceId     = null,
        string? referenceType = null)
    {
        return new NotificationLog
        {
            RecipientId   = recipientId,
            Title         = title,
            Message       = message,
            Channel       = channel,
            ReferenceId   = referenceId,
            ReferenceType = referenceType
        };
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SetUpdatedAt();
    }

    public void MarkAsFailed(string error)
    {
        Status       = NotificationStatus.Failed;
        ErrorMessage = error;
        SetUpdatedAt();
    }
}