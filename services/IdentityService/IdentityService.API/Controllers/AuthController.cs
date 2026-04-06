using IdentityService.Application.Features.Auth.Commands.Login;
using IdentityService.Application.Features.Auth.Commands.Register;
using IdentityService.Application.Features.Auth.Queries.GetProfile;
using IdentityService.Application.Features.Token.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, data = result });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetProfileQuery(userId), ct);
        return Ok(new { success = true, data = result });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // في الـ stateless JWT، الـ logout بيبقى client-side
        // لو عايز server-side blacklist نضيفه لاحقاً بـ Redis
        return Ok(new { success = true, message = "Logged out." });
    }
}