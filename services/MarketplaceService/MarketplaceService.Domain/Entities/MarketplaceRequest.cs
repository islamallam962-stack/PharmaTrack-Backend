using MarketplaceService.Domain.Common;
using MarketplaceService.Domain.Enums;
using MarketplaceService.Domain.Exceptions;

namespace MarketplaceService.Domain.Entities;

public class MarketplaceRequest : BaseEntity
{
    public Guid   BuyerPharmacyId { get; private set; }
    public string ProductName     { get; private set; } = default!;
    public int    QuantityNeeded  { get; private set; }
    public decimal MaxPrice       { get; private set; }
    public RequestStatus Status   { get; private set; } = RequestStatus.Open;

    // لما يتطابق مع listing
    public Guid? MatchedListingId { get; private set; }

    private MarketplaceRequest() { }

    public static MarketplaceRequest Create(
        Guid buyerPharmacyId,
        string productName,
        int quantityNeeded,
        decimal maxPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        if (quantityNeeded <= 0)
            throw new DomainException("Quantity needed must be greater than zero.");

        return new MarketplaceRequest
        {
            BuyerPharmacyId = buyerPharmacyId,
            ProductName     = productName.Trim(),
            QuantityNeeded  = quantityNeeded,
            MaxPrice        = maxPrice
        };
    }

    public void Match(Guid listingId)
    {
        if (Status != RequestStatus.Open)
            throw new DomainException("Only open requests can be matched.");

        MatchedListingId = listingId;
        Status           = RequestStatus.Matched;
        SetUpdatedAt();
    }

    public void Fulfill()
    {
        Status = RequestStatus.Fulfilled;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == RequestStatus.Fulfilled)
            throw new DomainException("Cannot cancel a fulfilled request.");

        Status = RequestStatus.Cancelled;
        SetUpdatedAt();
    }
}