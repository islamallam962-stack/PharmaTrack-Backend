using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Application.DTOs;
using MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Queries.GetAvailableListings;

public class GetAvailableListingsQueryHandler
    : IRequestHandler<GetAvailableListingsQuery, PagedResult<ListingDto>>
{
    private readonly IListingRepository _listings;

    public GetAvailableListingsQueryHandler(IListingRepository listings)
        => _listings = listings;

    public async Task<PagedResult<ListingDto>> Handle(
        GetAvailableListingsQuery request, CancellationToken ct)
    {
        var total = await _listings.CountAvailableAsync(request.ProductName, ct);
        var items = await _listings.GetAvailableAsync(
            request.ProductName, request.Page, request.PageSize, ct);

        var dtos = items.Select(CreateListingCommandHandler.ToDto).ToList();

        return new PagedResult<ListingDto>(
            dtos, total, request.Page, request.PageSize);
    }
}