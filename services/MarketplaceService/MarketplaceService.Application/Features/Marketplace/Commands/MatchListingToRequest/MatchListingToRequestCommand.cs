using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.MatchListingToRequest;

public record MatchListingToRequestCommand(
    Guid ListingId,
    Guid RequestId
) : IRequest<string>;