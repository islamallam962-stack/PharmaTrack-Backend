namespace MarketplaceService.Domain.Enums;

public enum RequestStatus
{
    Open      = 1,  // بيدور على دواء
    Matched   = 2,  // لقى عرض
    Fulfilled = 3,  // اتنفّذ
    Cancelled = 4
}