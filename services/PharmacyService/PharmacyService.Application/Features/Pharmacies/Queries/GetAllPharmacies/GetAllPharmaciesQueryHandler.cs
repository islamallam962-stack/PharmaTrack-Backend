using MediatR;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Application.DTOs;
using PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;

namespace PharmacyService.Application.Features.Pharmacies.Queries.GetAllPharmacies;

public class GetAllPharmaciesQueryHandler
    : IRequestHandler<GetAllPharmaciesQuery, PagedResult<PharmacyDto>>
{
    private readonly IPharmacyRepository _repo;

    public GetAllPharmaciesQueryHandler(IPharmacyRepository repo)
        => _repo = repo;

    public async Task<PagedResult<PharmacyDto>> Handle(
        GetAllPharmaciesQuery request,
        CancellationToken ct)
    {
        var total     = await _repo.CountAsync(ct);
        var items     = await _repo.GetAllAsync(request.Page, request.PageSize, ct);
        var dtos      = items.Select(CreatePharmacyCommandHandler.ToDto).ToList();

        return new PagedResult<PharmacyDto>(
            dtos, total, request.Page, request.PageSize);
    }
}