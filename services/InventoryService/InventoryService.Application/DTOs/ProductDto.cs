namespace InventoryService.Application.DTOs;

public record ProductDto(
    Guid           Id,
    string         Name,
    string?        ScientificName,
    string?        Manufacturer,
    string?        Category,
    Guid           PharmacyId,
    DateTime       CreatedAt,
    List<BatchDto> Batches
);