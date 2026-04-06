using InventoryService.Domain.Common;
using InventoryService.Domain.Enums;
using InventoryService.Domain.Exceptions;

namespace InventoryService.Domain.Entities;

public class ProductBatch : BaseEntity
{
    public string BatchNumber { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal SellingPrice { get; private set; }
    public DateTime ProductionDate { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public string? QrCode { get; private set; }
    public BatchStatus Status { get; private set; } = BatchStatus.Active;

    // FK
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    private ProductBatch() { }

    public static ProductBatch Create(
        string batchNumber,
        int quantity,
        decimal purchasePrice,
        decimal sellingPrice,
        DateTime productionDate,
        DateTime expiryDate,
        Guid productId)
    {
        if (string.IsNullOrWhiteSpace(batchNumber))
            throw new DomainException("Batch number is required.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (expiryDate <= DateTime.UtcNow)
            throw new DomainException("Expiry date must be in the future.");

        if (expiryDate <= productionDate)
            throw new DomainException("Expiry date must be after production date.");

        var batch = new ProductBatch
        {
            BatchNumber    = batchNumber.Trim().ToUpper(),
            Quantity       = quantity,
            PurchasePrice  = purchasePrice,
            SellingPrice   = sellingPrice,
            ProductionDate = productionDate,
            ExpiryDate     = expiryDate,
            ProductId      = productId
        };

        // تحديد الـ status تلقائياً
        batch.RefreshStatus();
        return batch;
    }

    public void SetQrCode(string qrCode)
    {
        QrCode = qrCode;
        SetUpdatedAt();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new DomainException("Quantity cannot be negative.");

        Quantity = newQuantity;
        SetUpdatedAt();
    }

    public void MarkAsListed()
    {
        Status = BatchStatus.Listed;
        SetUpdatedAt();
    }

    public void RefreshStatus()
    {
        var daysToExpiry = (ExpiryDate - DateTime.UtcNow).TotalDays;

        Status = daysToExpiry <= 0   ? BatchStatus.Expired
               : daysToExpiry <= 90  ? BatchStatus.NearExpiry
               : BatchStatus.Active;
    }
}