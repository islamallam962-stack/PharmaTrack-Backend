using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.AddProductManual;

public record AddProductManualCommand(
    string   Name,
    Guid     PharmacyId,
    string?  ScientificName,
    string?  Manufacturer,
    string?  Category,
    // Batch data
    string   BatchNumber,
    int      Quantity,
    decimal  PurchasePrice,
    decimal  SellingPrice,
    DateTime ProductionDate,
    DateTime ExpiryDate
) : IRequest<ProductDto>;