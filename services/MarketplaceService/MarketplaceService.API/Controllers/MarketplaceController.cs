using MarketplaceService.Application.Features.Marketplace.Commands.CreateListing;
using MarketplaceService.Application.Features.Marketplace.Commands.CreateRequest;
using MarketplaceService.Application.Features.Marketplace.Commands.MatchListingToRequest;
using MarketplaceService.Application.Features.Marketplace.Queries.GetAvailableListings;
using MarketplaceService.Application.Features.Marketplace.Queries.GetMyListings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceService.API.Controllers;

[ApiController]
[Route("api/marketplace")]
[Authorize]
public class MarketplaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public MarketplaceController(IMediator mediator) => _mediator = mediator;

    // صيدلية تعرض دواء هيتسبق بخصم 20%
    [HttpPost("listings")]
    public async Task<IActionResult> CreateListing(
        CreateListingCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAvailableListings),
            new { }, new { success = true, data = result });
    }

    // عرض كل الأدوية المتاحة في الـ Marketplace
    [HttpGet("listings")]
    public async Task<IActionResult> GetAvailableListings(
        [FromQuery] string? productName,
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct     = default)
    {
        var result = await _mediator.Send(
            new GetAvailableListingsQuery(productName, page, pageSize), ct);
        return Ok(new { success = true, data = result });
    }

    // عرض listings الصيدلية نفسها
    [HttpGet("listings/mine/{pharmacyId:guid}")]
    public async Task<IActionResult> GetMyListings(
        Guid pharmacyId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetMyListingsQuery(pharmacyId), ct);
        return Ok(new { success = true, data = result });
    }

    // صيدلية تطلب دواء ناقصها
    [HttpPost("requests")]
    public async Task<IActionResult> CreateRequest(
        CreateRequestCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, data = result });
    }

    // ربط عرض بطلب (الـ matching)
    [HttpPost("match")]
    public async Task<IActionResult> Match(
        MatchListingToRequestCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, message = result });
    }
}