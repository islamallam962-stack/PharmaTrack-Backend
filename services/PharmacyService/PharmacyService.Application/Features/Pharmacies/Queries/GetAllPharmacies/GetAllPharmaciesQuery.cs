using MediatR;
using PharmacyService.Application.DTOs;

namespace PharmacyService.Application.Features.Pharmacies.Queries.GetAllPharmacies;

public record GetAllPharmaciesQuery(
    int Page     = 1,
    int PageSize = 10
) : IRequest<PagedResult<PharmacyDto>>;