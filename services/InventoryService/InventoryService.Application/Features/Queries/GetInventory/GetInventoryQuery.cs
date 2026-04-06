using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Features.Products.Queries.GetInventory;

public record GetInventoryQuery(
    Guid PharmacyId,
    int  Page     = 1,
    int  PageSize = 20
) : IRequest<PagedResult<ProductDto>>;