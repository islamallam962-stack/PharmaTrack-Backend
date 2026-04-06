using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Application.DTOs;
using MarketplaceService.Domain.Entities;
using MarketplaceService.Domain.Exceptions;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;

public class CreateListingCommandHandler
    : IRequestHandler<CreateListingCommand, ListingDto>
{
    private readonly IListingRepository _listings;
    private readonly IUnitOfWork        _uow;

    public CreateListingCommandHandler(
        IListingRepository listings, IUnitOfWork uow)
    {
        _listings = listings;
        _uow      = uow;
    }

    public async Task<ListingDto> Handle(
        CreateListingCommand request, CancellationToken ct)
    {
        // منع نفس الـ batch يتعرض مرتين
        if (await _listings.BatchAlreadyListedAsync(request.BatchId, ct))
            throw new DomainException(
                "This batch is already listed on the marketplace.");

        var listing = MarketplaceListing.Create(
            request.SellerPharmacyId,
            request.BatchId,
            request.ProductName,
            request.BatchNumber,
            request.QuantityAvailable,
            request.OriginalPrice,
            request.ExpiryDate,
            request.Notes);

        await _listings.AddAsync(listing, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(listing);
    }

    public static ListingDto ToDto(MarketplaceListing l) => new(
        l.Id,
        l.SellerPharmacyId,
        l.ProductName,
        l.BatchNumber,
        l.QuantityAvailable,
        l.OriginalPrice,
        l.DiscountedPrice,
        20m,
        l.ExpiryDate,
        l.DaysToExpiry,
        l.Status.ToString(),
        l.Notes,
        l.CreatedAt);
}