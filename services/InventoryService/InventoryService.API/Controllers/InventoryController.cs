using InventoryService.Application.Features.Products.Commands.AddProductManual;
using InventoryService.Application.Features.Products.Commands.DeleteProduct;
using InventoryService.Application.Features.Products.Commands.ScanQrProduct;
using InventoryService.Application.Features.Products.Commands.UpdateStock;
using InventoryService.Application.Features.Products.Queries.GetInventory;
using InventoryService.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator) => _mediator = mediator;

    // إضافة منتج يدوي
    [HttpPost("manual")]
    public async Task<IActionResult> AddManual(
        AddProductManualCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById),
            new { id = result.Id },
            new { success = true, data = result });
    }

    // scan QR وجيب بيانات الـ batch
    [HttpPost("scan")]
    public async Task<IActionResult> ScanQr(
        ScanQrProductCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new { success = true, data = result });
    }

    // عرض الجرد الكامل للصيدلية
    [HttpGet("{pharmacyId:guid}")]
    public async Task<IActionResult> GetInventory(
        Guid pharmacyId,
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct     = default)
    {
        var result = await _mediator.Send(
            new GetInventoryQuery(pharmacyId, page, pageSize), ct);
        return Ok(new { success = true, data = result });
    }

    // عرض منتج بالـ ID
    [HttpGet("product/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return Ok(new { success = true, data = result });
    }

    // تحديث الكمية
    [HttpPatch("batch/{batchId:guid}/stock")]
    public async Task<IActionResult> UpdateStock(
        Guid batchId,
        [FromBody] int newQuantity,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new UpdateStockCommand(batchId, newQuantity), ct);
        return Ok(new { success = true, message = result });
    }

    // حذف منتج
    [HttpDelete("product/{id:guid}")]
    [Authorize(Roles = "PharmacyAdmin,SuperAdmin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id), ct);
        return Ok(new { success = true, message = result });
    }
}