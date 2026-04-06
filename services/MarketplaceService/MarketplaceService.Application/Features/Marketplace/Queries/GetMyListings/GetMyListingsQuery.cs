using MarketplaceService.Application.DTOs;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Queries.GetMyListings;

public record GetMyListingsQuery(Guid PharmacyId) : IRequest<List<ListingDto>>;