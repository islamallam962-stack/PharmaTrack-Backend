using MediatR;
using PharmacyService.Application.DTOs;

namespace PharmacyService.Application.Features.Pharmacies.Commands.UpdatePharmacy;

public record UpdatePharmacyCommand(
    Guid   PharmacyId,
    string Name,
    string OwnerName,
    string Email,
    string Phone
) : IRequest<PharmacyDto>;