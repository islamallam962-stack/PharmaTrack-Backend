namespace IdentityService.Application.DTOs;

public record AuthResponseDto(
    Guid   UserId,
    string FullName,
    string Email,
    string Role,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);