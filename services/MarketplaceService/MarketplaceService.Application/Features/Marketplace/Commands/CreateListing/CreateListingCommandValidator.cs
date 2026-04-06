using FluentValidation;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;

public class CreateListingCommandValidator
    : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.SellerPharmacyId).NotEmpty();
        RuleFor(x => x.BatchId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.BatchNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.QuantityAvailable).GreaterThan(0);
        RuleFor(x => x.OriginalPrice).GreaterThan(0);
        RuleFor(x => x.ExpiryDate).GreaterThan(DateTime.UtcNow)
            .WithMessage("Cannot list an expired product.");
    }
}