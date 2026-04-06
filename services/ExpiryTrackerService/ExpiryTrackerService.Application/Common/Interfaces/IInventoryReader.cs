namespace ExpiryTrackerService.Application.Common.Interfaces;

// ده بيقرأ من الـ Inventory DB مباشرة
// في الـ microservices الكبيرة بنعمل HTTP call
// بس احنا بنستخدم shared PostgreSQL فبنقرأ من الـ schema التاني
public interface IInventoryReader
{
    Task<List<NearExpiryBatchDto>> GetNearExpiryBatchesAsync(
        int daysThreshold,
        CancellationToken ct = default);
}

public record NearExpiryBatchDto(
    Guid     BatchId,
    Guid     ProductId,
    Guid     PharmacyId,
    string   ProductName,
    string   BatchNumber,
    DateTime ExpiryDate,
    int      Quantity
);