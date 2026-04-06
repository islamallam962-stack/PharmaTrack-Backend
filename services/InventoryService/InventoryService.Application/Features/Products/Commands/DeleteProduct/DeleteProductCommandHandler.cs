using InventoryService.Application.Common.Interfaces;
using InventoryService.Domain.Exceptions;
using MediatR;

namespace InventoryService.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, string>
{
    private readonly IProductRepository _products;
    private readonly IUnitOfWork        _uow;

    public DeleteProductCommandHandler(
        IProductRepository products,
        IUnitOfWork uow)
    {
        _products = products;
        _uow      = uow;
    }

    public async Task<string> Handle(
        DeleteProductCommand request,
        CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.ProductId, ct)
            ?? throw new DomainException("Product not found.");

        _products.Delete(product);
        await _uow.SaveChangesAsync(ct);

        return "Product deleted successfully.";
    }
}