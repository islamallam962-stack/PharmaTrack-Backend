using ExpiryTrackerService.Domain.Common;
using ExpiryTrackerService.Domain.Enums;

namespace ExpiryTrackerService.Domain.Entities;

public class ExpiryAlert : BaseEntity
{
    public Guid     PharmacyId   { get; private set; }
    public Guid     BatchId      { get; private set; }
    public string   ProductName  { get; private set; } = default!;
    public string   BatchNumber  { get; private set; } = default!;
    public DateTime ExpiryDate   { get; private set; }
    public int      DaysToExpiry { get; private set; }
    public AlertStatus Status    { get; private set; } = AlertStatus.Pending;

    private ExpiryAlert() { }

    public static ExpiryAlert Create(
        Guid pharmacyId,
        Guid batchId,
        string productName,
        string batchNumber,
        DateTime expiryDate)
    {
        var days = (int)(expiryDate - DateTime.UtcNow).TotalDays;

        return new ExpiryAlert
        {
            PharmacyId   = pharmacyId,
            BatchId      = batchId,
            ProductName  = productName,
            BatchNumber  = batchNumber,
            ExpiryDate   = expiryDate,
            DaysToExpiry = days
        };
    }

    public void MarkAsSent()
    {
        Status = AlertStatus.Sent;
        SetUpdatedAt();
    }

    public void MarkAsResolved()
    {
        Status = AlertStatus.Resolved;
        SetUpdatedAt();
    }
}