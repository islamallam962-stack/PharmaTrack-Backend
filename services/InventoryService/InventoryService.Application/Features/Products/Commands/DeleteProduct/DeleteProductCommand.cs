using MediatR;

namespace InventoryService.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest<string>;