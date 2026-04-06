namespace IdentityService.Application.DTOs;

public record UserProfileDto(
    Guid     UserId,
    string   FullName,
    string   Email,
    string   Role,
    bool     IsActive,
    DateTime CreatedAt
);