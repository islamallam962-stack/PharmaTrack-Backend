using MediatR;
using PharmacyService.Application.Common.Interfaces;
using PharmacyService.Application.DTOs;
using PharmacyService.Domain.Entities;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;

public class CreatePharmacyCommandHandler
    : IRequestHandler<CreatePharmacyCommand, PharmacyDto>
{
    private readonly IPharmacyRepository _repo;
    private readonly IUnitOfWork         _uow;

    public CreatePharmacyCommandHandler(
        IPharmacyRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow  = uow;
    }

    public async Task<PharmacyDto> Handle(
        CreatePharmacyCommand request,
        CancellationToken ct)
    {
        if (await _repo.ExistsByLicenseAsync(request.LicenseNumber, ct))
            throw new DomainException("License number already registered.");

        var pharmacy = Pharmacy.Create(
            request.Name,
            request.LicenseNumber,
            request.OwnerName,
            request.Email,
            request.Phone,
            request.OwnerId);

        // إضافة الفرع الرئيسي تلقائياً
        var mainBranch = PharmacyBranch.Create(
            "Main Branch",
            request.BranchAddress,
            request.BranchPhone,
            request.Latitude,
            request.Longitude,
            isMain: true,
            pharmacy.Id);

        pharmacy.AddBranch(mainBranch);

        await _repo.AddAsync(pharmacy, ct);
        await _uow.SaveChangesAsync(ct);

        return ToDto(pharmacy);
    }

    public static PharmacyDto ToDto(Pharmacy p) => new(
        p.Id,
        p.Name,
        p.LicenseNumber,
        p.OwnerName,
        p.Email,
        p.Phone,
        p.Status.ToString(),
        p.CreatedAt,
        p.Branches.Select(b => new PharmacyBranchDto(
            b.Id, b.Name, b.Address,
            b.Phone, b.Latitude, b.Longitude, b.IsMain
        )).ToList());
}