using FluentValidation;

namespace PharmacyService.Application.Features.Pharmacies.Commands.UpdatePharmacy;

public class UpdatePharmacyCommandValidator
    : AbstractValidator<UpdatePharmacyCommand>
{
    public UpdatePharmacyCommandValidator()
    {
        RuleFor(x => x.PharmacyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
    }
}