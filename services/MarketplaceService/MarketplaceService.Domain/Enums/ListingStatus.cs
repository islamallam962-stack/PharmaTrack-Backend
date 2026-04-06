namespace MarketplaceService.Domain.Enums;

public enum ListingStatus
{
    Active    = 1,  // معروض للبيع
    Matched   = 2,  // اتطابق مع طلب
    Sold      = 3,  // اتباع
    Cancelled = 4   // اتلغى
}