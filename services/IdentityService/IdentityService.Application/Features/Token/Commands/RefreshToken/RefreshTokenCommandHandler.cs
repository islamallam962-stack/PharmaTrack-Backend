using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Exceptions;
using MediatR;

namespace IdentityService.Application.Features.Token.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtService     _jwt;
    private readonly IUnitOfWork     _uow;

    public RefreshTokenCommandHandler(
        IUserRepository users,
        IJwtService jwt,
        IUnitOfWork uow)
    {
        _users = users;
        _jwt   = jwt;
        _uow   = uow;
    }

    public async Task<AuthResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken ct)
    {
        var userId = _jwt.GetUserIdFromExpiredToken(request.AccessToken)
            ?? throw new DomainException("Invalid access token.");

        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new DomainException("User not found.");

        if (user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new DomainException("Invalid or expired refresh token.");

        var newAccess  = _jwt.GenerateAccessToken(user);
        var newRefresh = _jwt.GenerateRefreshToken();

        user.SetRefreshToken(newRefresh, DateTime.UtcNow.AddDays(7));
        _users.Update(user);
        await _uow.SaveChangesAsync(ct);

        return new AuthResponseDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role.ToString(),
            newAccess,
            newRefresh,
            DateTime.UtcNow.AddMinutes(60));
    }
}