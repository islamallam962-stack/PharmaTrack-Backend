using MarketplaceService.Domain.Common;
using MarketplaceService.Domain.Enums;
using MarketplaceService.Domain.Exceptions;

namespace MarketplaceService.Domain.Entities;

public class MarketplaceListing : BaseEntity
{
    public Guid   SellerPharmacyId { get; private set; }
    public Guid   BatchId          { get; private set; }
    public string ProductName      { get; private set; } = default!;
    public string BatchNumber      { get; private set; } = default!;
    public int    QuantityAvailable { get; private set; }
    public decimal OriginalPrice   { get; private set; }
    public decimal DiscountedPrice { get; private set; }
    public DateTime ExpiryDate     { get; private set; }
    public int    DaysToExpiry     { get; private set; }
    public ListingStatus Status    { get; private set; } = ListingStatus.Active;
    public string? Notes           { get; private set; }

    private MarketplaceListing() { }

    public static MarketplaceListing Create(
        Guid sellerPharmacyId,
        Guid batchId,
        string productName,
        string batchNumber,
        int quantityAvailable,
        decimal originalPrice,
        DateTime expiryDate,
        string? notes = null)
    {
        if (quantityAvailable <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (originalPrice <= 0)
            throw new DomainException("Price must be greater than zero.");

        var daysToExpiry = (int)(expiryDate - DateTime.UtcNow).TotalDays;
        if (daysToExpiry <= 0)
            throw new DomainException("Cannot list an already expired product.");

        // الخصم 20% تلقائي
        var discountedPrice = Math.Round(originalPrice * 0.80m, 2);

        return new MarketplaceListing
        {
            SellerPharmacyId  = sellerPharmacyId,
            BatchId           = batchId,
            ProductName       = productName.Trim(),
            BatchNumber       = batchNumber.Trim().ToUpper(),
            QuantityAvailable = quantityAvailable,
            OriginalPrice     = originalPrice,
            DiscountedPrice   = discountedPrice,
            ExpiryDate        = expiryDate,
            DaysToExpiry      = daysToExpiry,
            Notes             = notes?.Trim()
        };
    }

    public void MarkAsMatched()
    {
        if (Status != ListingStatus.Active)
            throw new DomainException("Only active listings can be matched.");

        Status = ListingStatus.Matched;
        SetUpdatedAt();
    }

    public void MarkAsSold()
    {
        Status = ListingStatus.Sold;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == ListingStatus.Sold)
            throw new DomainException("Cannot cancel a sold listing.");

        Status = ListingStatus.Cancelled;
        SetUpdatedAt();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new DomainException("Quantity cannot be negative.");

        QuantityAvailable = newQuantity;
        if (newQuantity == 0) MarkAsSold();
        SetUpdatedAt();
    }
}