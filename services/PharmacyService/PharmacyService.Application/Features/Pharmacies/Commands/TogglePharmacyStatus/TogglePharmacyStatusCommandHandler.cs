using MediatR;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Application.Features.Pharmacies.Commands.TogglePharmacyStatus;

public class TogglePharmacyStatusCommandHandler
    : IRequestHandler<TogglePharmacyStatusCommand, string>
{
    private readonly IPharmacyRepository _repo;
    private readonly IUnitOfWork         _uow;

    public TogglePharmacyStatusCommandHandler(
        IPharmacyRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<string> Handle(
        TogglePharmacyStatusCommand request,
        CancellationToken ct)
    {
        var pharmacy = await _repo.GetByIdAsync(request.PharmacyId, ct)
            ?? throw new DomainException("Pharmacy not found.");

        switch (request.Action.ToLower())
        {
            case "activate": pharmacy.Activate(); break;
            case "suspend":  pharmacy.Suspend();  break;
            default: throw new DomainException("Invalid action. Use 'activate' or 'suspend'.");
        }

        _repo.Update(pharmacy);
        await _uow.SaveChangesAsync(ct);

        return pharmacy.Status.ToString();
    }
}