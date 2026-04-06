using IdentityService.Application.DTOs;
using MediatR;

namespace IdentityService.Application.Features.Auth.Queries.GetProfile;

public record GetProfileQuery(Guid UserId) : IRequest<UserProfileDto>;