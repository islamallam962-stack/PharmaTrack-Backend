namespace MarketplaceService.Application.DTOs;

public record ListingDto(
    Guid     Id,
    Guid     SellerPharmacyId,
    string   ProductName,
    string   BatchNumber,
    int      QuantityAvailable,
    decimal  OriginalPrice,
    decimal  DiscountedPrice,
    decimal  DiscountPercent,
    DateTime ExpiryDate,
    int      DaysToExpiry,
    string   Status,
    string?  Notes,
    DateTime CreatedAt
);