using MediatR;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Application.DTOs;
using PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Application.Features.Pharmacies.Commands.UpdatePharmacy;

public class UpdatePharmacyCommandHandler
    : IRequestHandler<UpdatePharmacyCommand, PharmacyDto>
{
    private readonly IPharmacyRepository _repo;
    private readonly IUnitOfWork         _uow;

    public UpdatePharmacyCommandHandler(
        IPharmacyRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<PharmacyDto> Handle(
        UpdatePharmacyCommand request,
        CancellationToken ct)
    {
        var pharmacy = await _repo.GetByIdAsync(request.PharmacyId, ct)
            ?? throw new DomainException("Pharmacy not found.");

        pharmacy.Update(
            request.Name,
            request.OwnerName,
            request.Email,
            request.Phone);

        _repo.Update(pharmacy);
        await _uow.SaveChangesAsync(ct);

        return CreatePharmacyCommandHandler.ToDto(pharmacy);
    }
}