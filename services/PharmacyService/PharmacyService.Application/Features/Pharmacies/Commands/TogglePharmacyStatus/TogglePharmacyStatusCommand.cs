using MediatR;

namespace PharmacyService.Application.Features.Pharmacies.Commands.TogglePharmacyStatus;

public record TogglePharmacyStatusCommand(
    Guid   PharmacyId,
    string Action        // "activate" | "suspend"
) : IRequest<string>;