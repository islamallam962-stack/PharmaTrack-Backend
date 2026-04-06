using IdentityService.Application.DTOs;
using IdentityService.Domain.Enums;
using MediatR;

namespace IdentityService.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string   FullName,
    string   Email,
    string   Password,
    UserRole Role,
    Guid?    EntityId
) : IRequest<AuthResponseDto>;