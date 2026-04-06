namespace ExpiryTrackerService.Domain.Enums;

public enum AlertStatus
{
    Pending  = 1,   // اتعملت لسه مابعتتش
    Sent     = 2,   // اتبعتت
    Resolved = 3    // الصيدلي عمل حاجة (باع أو رفع على الـ marketplace)
}