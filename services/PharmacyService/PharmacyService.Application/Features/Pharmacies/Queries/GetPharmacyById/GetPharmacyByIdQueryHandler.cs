using MediatR;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Application.DTOs;
using PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Application.Features.Pharmacies.Queries.GetPharmacyById;

public class GetPharmacyByIdQueryHandler
    : IRequestHandler<GetPharmacyByIdQuery, PharmacyDto>
{
    private readonly IPharmacyRepository _repo;

    public GetPharmacyByIdQueryHandler(IPharmacyRepository repo)
        => _repo = repo;

    public async Task<PharmacyDto> Handle(
        GetPharmacyByIdQuery request,
        CancellationToken ct)
    {
        var pharmacy = await _repo.GetByIdAsync(request.PharmacyId, ct)
            ?? throw new DomainException("Pharmacy not found.");

        return CreatePharmacyCommandHandler.ToDto(pharmacy);
    }
}