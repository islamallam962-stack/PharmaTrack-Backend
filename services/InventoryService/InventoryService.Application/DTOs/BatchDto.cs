namespace InventoryService.Application.DTOs;

public record BatchDto(
    Guid     Id,
    string   BatchNumber,
    int      Quantity,
    decimal  PurchasePrice,
    decimal  SellingPrice,
    DateTime ProductionDate,
    DateTime ExpiryDate,
    string   Status,
    string?  QrCode,
    int      DaysToExpiry
);