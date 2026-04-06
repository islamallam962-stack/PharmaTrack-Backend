using InventoryService.Domain.Common;
using InventoryService.Domain.Exceptions;

namespace InventoryService.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string? ScientificName { get; private set; }
    public string? Manufacturer { get; private set; }
    public string? Category { get; private set; }

    // اللي بتمتلك المنتج ده
    public Guid PharmacyId { get; private set; }

    private readonly List<ProductBatch> _batches = new();
    public IReadOnlyCollection<ProductBatch> Batches => _batches.AsReadOnly();

    private Product() { }

    public static Product Create(
        string name,
        Guid pharmacyId,
        string? scientificName = null,
        string? manufacturer   = null,
        string? category       = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");

        return new Product
        {
            Name           = name.Trim(),
            PharmacyId     = pharmacyId,
            ScientificName = scientificName?.Trim(),
            Manufacturer   = manufacturer?.Trim(),
            Category       = category?.Trim()
        };
    }

    public void Update(string name, string? scientificName,
                       string? manufacturer, string? category)
    {
        Name           = name.Trim();
        ScientificName = scientificName?.Trim();
        Manufacturer   = manufacturer?.Trim();
        Category       = category?.Trim();
        SetUpdatedAt();
    }

    public void AddBatch(ProductBatch batch)
    {
        _batches.Add(batch);
        SetUpdatedAt();
    }
}