using MediatR;
using PharmacyService.Application.DTOs;

namespace PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;

public record CreatePharmacyCommand(
    string Name,
    string LicenseNumber,
    string OwnerName,
    string Email,
    string Phone,
    Guid   OwnerId,
    // الفرع الرئيسي بيتسجل مع الصيدلية
    string BranchAddress,
    string BranchPhone,
    double Latitude,
    double Longitude
) : IRequest<PharmacyDto>;