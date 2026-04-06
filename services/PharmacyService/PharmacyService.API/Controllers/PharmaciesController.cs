using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;
using PharmacyService.Application.Features.Pharmacies.Commands.TogglePharmacyStatus;
using PharmacyService.Application.Features.Pharmacies.Commands.UpdatePharmacy;
using PharmacyService.Application.Features.Pharmacies.Queries.GetAllPharmacies;
using PharmacyService.Application.Features.Pharmacies.Queries.GetPharmacyById;
using System.Security.Claims;

namespace PharmacyService.API.Controllers;

[ApiController]
[Route("api/pharmacies")]
[Authorize]
public class PharmaciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PharmaciesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePharmacyCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Id },
            new { success = true, data = result });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPharmacyByIdQuery(id), ct);
        return Ok(new { success = true, data = result });
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetAllPharmaciesQuery(page, pageSize), ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, UpdatePharmacyCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(
            command with { PharmacyId = id }, ct);
        return Ok(new { success = true, data = result });
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> ToggleStatus(
        Guid id, [FromBody] string action, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new TogglePharmacyStatusCommand(id, action), ct);
        return Ok(new { success = true, status = result });
    }
}