using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDto>;