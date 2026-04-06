using MediatR;
using PharmacyService.Application.DTOs;

namespace PharmacyService.Application.Features.Pharmacies.Queries.GetPharmacyById;

public record GetPharmacyByIdQuery(Guid PharmacyId) : IRequest<PharmacyDto>;