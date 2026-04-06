using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Exceptions;
using MediatR;
using BCrypt.Net;
namespace IdentityService.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtService     _jwt;
    private readonly IUnitOfWork     _uow;

    public LoginCommandHandler(
        IUserRepository users,
        IJwtService jwt,
        IUnitOfWork uow)
    {
        _users = users;
        _jwt   = jwt;
        _uow   = uow;
    }

    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct)
            ?? throw new DomainException("Invalid credentials.");

        if (!user.IsActive)
            throw new DomainException("Account is deactivated.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Invalid credentials.");

        var accessToken  = _jwt.GenerateAccessToken(user);
        var refreshToken = _jwt.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        _users.Update(user);
        await _uow.SaveChangesAsync(ct);

        return new AuthResponseDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role.ToString(),
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(60));
    }
}