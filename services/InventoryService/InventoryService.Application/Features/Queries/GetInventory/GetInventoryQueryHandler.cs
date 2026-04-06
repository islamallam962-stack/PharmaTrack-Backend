using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.DTOs;
using InventoryService.Application.Features.Products.Commands.AddProductManual;
using MediatR;

namespace InventoryService.Application.Features.Products.Queries.GetInventory;

public class GetInventoryQueryHandler
    : IRequestHandler<GetInventoryQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _products;

    public GetInventoryQueryHandler(IProductRepository products)
        => _products = products;

    public async Task<PagedResult<ProductDto>> Handle(
        GetInventoryQuery request,
        CancellationToken ct)
    {
        var total = await _products.CountByPharmacyAsync(request.PharmacyId, ct);
        var items = await _products.GetByPharmacyAsync(
                        request.PharmacyId, request.Page, request.PageSize, ct);

        var dtos = items.Select(AddProductManualCommandHandler.ToDto).ToList();

        return new PagedResult<ProductDto>(
            dtos, total, request.Page, request.PageSize);
    }
}