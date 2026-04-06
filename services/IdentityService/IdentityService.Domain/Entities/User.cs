using IdentityService.Domain.Common;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Exceptions;

namespace IdentityService.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiry { get; private set; }

    // اللي بيمثل انتماء اليوزر للـ entity بتاعته
    public Guid? EntityId { get; private set; }

    private User() { }

    public static User Create(
        string fullName,
        string email,
        string passwordHash,
        UserRole role,
        Guid? entityId = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        return new User
        {
            FullName     = fullName.Trim(),
            Email        = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role         = role,
            EntityId     = entityId
        };
    }

    public void SetRefreshToken(string token, DateTime expiry)
    {
        RefreshToken       = token;
        RefreshTokenExpiry = expiry;
        SetUpdatedAt();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken       = null;
        RefreshTokenExpiry = null;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}