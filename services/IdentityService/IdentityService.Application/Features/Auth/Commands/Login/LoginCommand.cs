using IdentityService.Application.DTOs;
using MediatR;

namespace IdentityService.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;