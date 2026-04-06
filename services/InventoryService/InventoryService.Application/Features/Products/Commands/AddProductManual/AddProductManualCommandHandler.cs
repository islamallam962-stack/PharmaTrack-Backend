using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.AddProductManual;

public class AddProductManualCommandHandler
    : IRequestHandler<AddProductManualCommand, ProductDto>
{
    private readonly IProductRepository _products;
    private readonly IBatchRepository   _batches;
    private readonly IQrService         _qr;
    private readonly IUnitOfWork        _uow;

    public AddProductManualCommandHandler(
        IProductRepository products,
        IBatchRepository batches,
        IQrService qr,
        IUnitOfWork uow)
    {
        _products = products;
        _batches  = batches;
        _qr       = qr;
        _uow      = uow;
    }

    public async Task<ProductDto> Handle(
        AddProductManualCommand request,
        CancellationToken ct)
    {
        // لو المنتج موجود قبل كده نضيف batch جديدة بس
        var product = await _products.GetByNameAndPharmacyAsync(
                          request.Name, request.PharmacyId, ct);

        if (product is null)
        {
            product = Product.Create(
                request.Name,
                request.PharmacyId,
                request.ScientificName,
                request.Manufacturer,
                request.Category);

            await _products.AddAsync(product, ct);
        }

        var batch = ProductBatch.Create(
            request.BatchNumber,
            request.Quantity,
            request.PurchasePrice,
            request.SellingPrice,
            request.ProductionDate,
            request.ExpiryDate,
            product.Id);

        // توليد QR تلقائي لكل batch
        var qrData   = $"PHARMA|{product.Id}|{batch.Id}|{batch.BatchNumber}";
        var qrBase64 = _qr.GenerateQrCode(qrData);
        batch.SetQrCode(qrBase64);

        product.AddBatch(batch);
        await _batches.AddAsync(batch, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(product);
    }

    public static ProductDto ToDto(Product p) => new(
        p.Id,
        p.Name,
        p.ScientificName,
        p.Manufacturer,
        p.Category,
        p.PharmacyId,
        p.CreatedAt,
        p.Batches.Select(b => new BatchDto(
            b.Id,
            b.BatchNumber,
            b.Quantity,
            b.PurchasePrice,
            b.SellingPrice,
            b.ProductionDate,
            b.ExpiryDate,
            b.Status.ToString(),
            b.QrCode,
            (int)(b.ExpiryDate - DateTime.UtcNow).TotalDays
        )).ToList());
}