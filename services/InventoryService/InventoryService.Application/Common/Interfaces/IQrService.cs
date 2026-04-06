namespace InventoryService.Application.Common.Interfaces;

public interface IQrService
{
    // بيولّد QR ويرجع Base64 string
    string GenerateQrCode(string data);

    // بيفك الـ QR ويرجع الـ data جوه
    string DecodeQrCode(string base64Image);
}