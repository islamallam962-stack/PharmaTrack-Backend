namespace NotificationService.Domain.Enums;

[Flags]
public enum NotificationChannel
{
    None      = 0,
    Dashboard = 1,
    Email     = 2,
    Push      = 4,
    Sms       = 8
}