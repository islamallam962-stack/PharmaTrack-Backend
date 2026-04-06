namespace MarketplaceService.Application.DTOs;

public record RequestDto(
    Guid     Id,
    Guid     BuyerPharmacyId,
    string   ProductName,
    int      QuantityNeeded,
    decimal  MaxPrice,
    string   Status,
    Guid?    MatchedListingId,
    DateTime CreatedAt
);