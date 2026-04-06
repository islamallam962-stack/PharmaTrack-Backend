using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Application.DTOs;
using MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Queries.GetMyListings;

public class GetMyListingsQueryHandler
    : IRequestHandler<GetMyListingsQuery, List<ListingDto>>
{
    private readonly IListingRepository _listings;

    public GetMyListingsQueryHandler(IListingRepository listings)
        => _listings = listings;

    public async Task<List<ListingDto>> Handle(
        GetMyListingsQuery request, CancellationToken ct)
    {
        var items = await _listings.GetBySellerAsync(request.PharmacyId, ct);
        return items.Select(CreateListingCommandHandler.ToDto).ToList();
    }
}