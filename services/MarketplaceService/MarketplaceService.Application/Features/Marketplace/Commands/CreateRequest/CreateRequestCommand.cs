using MarketplaceService.Application.DTOs;
using MediatR;

namespace MarketplaceService.Application.Features.Marketplace.Commands.CreateRequest;

public record CreateRequestCommand(
    Guid    BuyerPharmacyId,
    string  ProductName,
    int     QuantityNeeded,
    decimal MaxPrice
) : IRequest<RequestDto>;