using InventoryService.Application.Common.Interfaces;
using InventoryService.Domain.Exceptions;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.UpdateStock;

public class UpdateStockCommandHandler
    : IRequestHandler<UpdateStockCommand, string>
{
    private readonly IBatchRepository _batches;
    private readonly IUnitOfWork      _uow;

    public UpdateStockCommandHandler(
        IBatchRepository batches,
        IUnitOfWork uow)
    {
        _batches = batches;
        _uow     = uow;
    }

    public async Task<string> Handle(
        UpdateStockCommand request,
        CancellationToken ct)
    {
        var batch = await _batches.GetByIdAsync(request.BatchId, ct)
            ?? throw new DomainException("Batch not found.");

        batch.UpdateQuantity(request.NewQuantity);
        _batches.Update(batch);
        await _uow.SaveChangesAsync(ct);

        return $"Stock updated to {request.NewQuantity}.";
    }
}