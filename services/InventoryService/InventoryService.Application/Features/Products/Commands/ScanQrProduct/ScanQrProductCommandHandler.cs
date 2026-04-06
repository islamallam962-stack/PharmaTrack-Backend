using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Exceptions;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.ScanQrProduct;

public class ScanQrProductCommandHandler
    : IRequestHandler<ScanQrProductCommand, BatchDto>
{
    private readonly IBatchRepository _batches;
    private readonly IQrService       _qr;

    public ScanQrProductCommandHandler(
        IBatchRepository batches,
        IQrService qr)
    {
        _batches = batches;
        _qr      = qr;
    }

    public async Task<BatchDto> Handle(
        ScanQrProductCommand request,
        CancellationToken ct)
    {
        // فك الـ QR وجيب الـ data
        var data = _qr.DecodeQrCode(request.QrBase64);

        // الـ format: PHARMA|productId|batchId|batchNumber
        var parts = data.Split('|');
        if (parts.Length < 4 || parts[0] != "PHARMA")
            throw new DomainException("Invalid QR code format.");

        var batch = await _batches.GetByIdAsync(Guid.Parse(parts[2]), ct)
            ?? throw new DomainException("Batch not found.");

        batch.RefreshStatus();

        return new BatchDto(
            batch.Id,
            batch.BatchNumber,
            batch.Quantity,
            batch.PurchasePrice,
            batch.SellingPrice,
            batch.ProductionDate,
            batch.ExpiryDate,
            batch.Status.ToString(),
            batch.QrCode,
            (int)(batch.ExpiryDate - DateTime.UtcNow).TotalDays);
    }
}