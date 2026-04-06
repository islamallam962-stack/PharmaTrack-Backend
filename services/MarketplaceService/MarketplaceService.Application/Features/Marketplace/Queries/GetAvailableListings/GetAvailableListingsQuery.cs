using MarketplaceService.Application.DTOs;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Queries.GetAvailableListings;

public record GetAvailableListingsQuery(
    string? ProductName = null,
    int     Page        = 1,
    int     PageSize    = 20
) : IRequest<PagedResult<ListingDto>>;