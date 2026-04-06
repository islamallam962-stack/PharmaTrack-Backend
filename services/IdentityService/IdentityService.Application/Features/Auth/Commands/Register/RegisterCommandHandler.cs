using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Exceptions;
using MediatR;
using BCrypt.Net;
namespace IdentityService.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtService     _jwt;
    private readonly IUnitOfWork     _uow;

    public RegisterCommandHandler(
        IUserRepository users,
        IJwtService jwt,
        IUnitOfWork uow)
    {
        _users = users;
        _jwt   = jwt;
        _uow   = uow;
    }

    public async Task<AuthResponseDto> Handle(
        RegisterCommand request,
        CancellationToken ct)
    {
        if (await _users.ExistsByEmailAsync(request.Email, ct))
            throw new DomainException("Email already registered.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(
            request.FullName,
            request.Email,
            hash,
            request.Role,
            request.EntityId);

        var accessToken  = _jwt.GenerateAccessToken(user);
        var refreshToken = _jwt.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await _users.AddAsync(user, ct);
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