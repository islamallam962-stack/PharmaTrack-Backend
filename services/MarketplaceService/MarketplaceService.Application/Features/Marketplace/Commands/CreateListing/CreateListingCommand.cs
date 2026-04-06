using MarketplaceService.Application.DTOs;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;

public record CreateListingCommand(
    Guid     SellerPharmacyId,
    Guid     BatchId,
    string   ProductName,
    string   BatchNumber,
    int      QuantityAvailable,
    decimal  OriginalPrice,
    DateTime ExpiryDate,
    string?  Notes
) : IRequest<ListingDto>;