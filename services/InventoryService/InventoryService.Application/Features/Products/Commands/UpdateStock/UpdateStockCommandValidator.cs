using FluentValidation;

namespace InventoryService.Application.Features.Products.Commands.UpdateStock;

public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
{
    public UpdateStockCommandValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty();
        RuleFor(x => x.NewQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative.");
    }
}