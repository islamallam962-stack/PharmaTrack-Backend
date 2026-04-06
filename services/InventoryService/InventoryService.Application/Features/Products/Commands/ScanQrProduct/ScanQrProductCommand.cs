using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.ScanQrProduct;

public record ScanQrProductCommand(
    string QrBase64  // الـ QR image كـ base64
) : IRequest<BatchDto>;