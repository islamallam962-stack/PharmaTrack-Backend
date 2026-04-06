using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.DTOs;
using InventoryService.Application.Features.Products.Commands.AddProductManual;
using InventoryService.Domain.Exceptions;
using MediatR;

namespace InventoryService.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _products;

    public GetProductByIdQueryHandler(IProductRepository products)
        => _products = products;

    public async Task<ProductDto> Handle(
        GetProductByIdQuery request,
        CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.ProductId, ct)
            ?? throw new DomainException("Product not found.");

        return AddProductManualCommandHandler.ToDto(product);
    }
}