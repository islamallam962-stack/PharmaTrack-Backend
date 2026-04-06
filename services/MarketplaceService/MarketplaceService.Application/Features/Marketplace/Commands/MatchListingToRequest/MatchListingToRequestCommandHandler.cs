using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Domain.Exceptions;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.MatchListingToRequest;

public class MatchListingToRequestCommandHandler
    : IRequestHandler<MatchListingToRequestCommand, string>
{
    private readonly IListingRepository _listings;
    private readonly IRequestRepository _requests;
    private readonly IUnitOfWork        _uow;

    public MatchListingToRequestCommandHandler(
        IListingRepository listings,
        IRequestRepository requests,
        IUnitOfWork uow)
    {
        _listings = listings;
        _requests = requests;
        _uow      = uow;
    }

    public async Task<string> Handle(
        MatchListingToRequestCommand request, CancellationToken ct)
    {
        var listing = await _listings.GetByIdAsync(request.ListingId, ct)
            ?? throw new DomainException("Listing not found.");

        var req = await _requests.GetByIdAsync(request.RequestId, ct)
            ?? throw new DomainException("Request not found.");

        // التحقق إن المنتج متطابق
        if (!listing.ProductName.Equals(
                req.ProductName, StringComparison.OrdinalIgnoreCase))
            throw new DomainException(
                "Listing and request product names do not match.");

        // التحقق إن السعر في الحدود المقبولة للمشتري
        if (req.MaxPrice > 0 && listing.DiscountedPrice > req.MaxPrice)
            throw new DomainException(
                $"Listing price ({listing.DiscountedPrice}) exceeds " +
                $"buyer's max price ({req.MaxPrice}).");

        // التحقق إن الكمية كافية
        if (listing.QuantityAvailable < req.QuantityNeeded)
            throw new DomainException(
                $"Available quantity ({listing.QuantityAvailable}) is less " +
                $"than needed ({req.QuantityNeeded}).");

        // تنفيذ الـ match
        listing.MarkAsMatched();
        req.Match(listing.Id);

        _listings.Update(listing);
        _requests.Update(req);
        await _uow.SaveChangesAsync(ct);

        return $"Matched! Listing {listing.Id} → Request {req.Id}. " +
               $"Price: {listing.DiscountedPrice} EGP (20% off).";
    }
}