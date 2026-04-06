using IdentityService.Application.Common.Interfaces;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Exceptions;
using MediatR;

namespace IdentityService.Application.Features.Auth.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUserRepository _users;

    public GetProfileQueryHandler(IUserRepository users) => _users = users;

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.UserId, ct)
            ?? throw new DomainException("User not found.");

        return new UserProfileDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role.ToString(),
            user.IsActive,
            user.CreatedAt);
    }
}