using MarketplaceService.Application.Common.Interfaces;
using MarketplaceService.Application.DTOs;
using MarketplaceService.Domain.Entities;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateRequest;

public class CreateRequestCommandHandler
    : IRequestHandler<CreateRequestCommand, RequestDto>
{
    private readonly IRequestRepository _requests;
    private readonly IUnitOfWork        _uow;

    public CreateRequestCommandHandler(
        IRequestRepository requests, IUnitOfWork uow)
    {
        _requests = requests;
        _uow      = uow;
    }

    public async Task<RequestDto> Handle(
        CreateRequestCommand request, CancellationToken ct)
    {
        var req = MarketplaceRequest.Create(
            request.BuyerPharmacyId,
            request.ProductName,
            request.QuantityNeeded,
            request.MaxPrice);

        await _requests.AddAsync(req, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(req);
    }

    public static RequestDto ToDto(MarketplaceRequest r) => new(
        r.Id,
        r.BuyerPharmacyId,
        r.ProductName,
        r.QuantityNeeded,
        r.MaxPrice,
        r.Status.ToString(),
        r.MatchedListingId,
        r.CreatedAt);
}