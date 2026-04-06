namespace NotificationService.Application.DTOs;

public record NotificationLogDto(
    Guid     Id,
    string   Title,
    string   Message,
    string   Channel,
    string   Status,
    DateTime CreatedAt
);