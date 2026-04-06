using IdentityService.Application.DTOs;
using MediatR;

namespace IdentityService.Application.Features.Token.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<AuthResponseDto>;