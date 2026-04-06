using InventoryService.Application.Common.Interfaces;
using QRCoder;

namespace InventoryService.Infrastructure.Services;

public class QrService : IQrService
{
    public string GenerateQrCode(string data)
    {
        using var generator = new QRCodeGenerator();
        var qrData  = generator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var code = new PngByteQRCode(qrData);
        var bytes = code.GetGraphic(10);
        return Convert.ToBase64String(bytes);
    }

    public string DecodeQrCode(string base64Image)
    {
          // NOTE: QR decoding happens on the mobile/frontend side.
    	 // This endpoint expects the already-decoded string (e.g. "PHARMA|productId|batchId|batchNumber"),
    	// not a raw base64 image. The parameter name is misleading — consider renaming it in a future refactor.
        return base64Image;
    }
}