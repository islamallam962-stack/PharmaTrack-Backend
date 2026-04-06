namespace InventoryService.Domain.Enums;

public enum BatchStatus
{
    Active    = 1,  // كويس
    NearExpiry = 2, // فاضل 90 يوم أو أقل
    Expired   = 3,  // انتهى
    Listed    = 4   // متعروض في الـ Marketplace
}