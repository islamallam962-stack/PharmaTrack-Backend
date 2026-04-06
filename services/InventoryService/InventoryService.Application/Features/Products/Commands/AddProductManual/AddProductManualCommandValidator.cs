using FluentValidation;

namespace InventoryService.Application.Features.Products.Commands.AddProductManual;

public class AddProductManualCommandValidator
    : AbstractValidator<AddProductManualCommand>
{
    public AddProductManualCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.PharmacyId)
            .NotEmpty();

        RuleFor(x => x.BatchNumber)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0).WithMessage("Purchase price must be greater than zero.");

        RuleFor(x => x.SellingPrice)
            .GreaterThan(0).WithMessage("Selling price must be greater than zero.")
            .GreaterThanOrEqualTo(x => x.PurchasePrice)
            .WithMessage("Selling price must be >= purchase price.");

        RuleFor(x => x.ProductionDate)
            .LessThan(x => x.ExpiryDate)
            .WithMessage("Production date must be before expiry date.");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future.");
    }
}