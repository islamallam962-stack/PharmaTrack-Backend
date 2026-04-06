using MediatR;

namespace InventoryService.Application.Features.Products.Commands.UpdateStock;

public record UpdateStockCommand(
    Guid BatchId,
    int  NewQuantity
) : IRequest<string>;