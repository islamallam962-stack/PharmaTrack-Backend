using FluentValidation;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateRequest;

public class CreateRequestCommandValidator
    : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.BuyerPharmacyId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.QuantityNeeded).GreaterThan(0);
        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0);
    }
}