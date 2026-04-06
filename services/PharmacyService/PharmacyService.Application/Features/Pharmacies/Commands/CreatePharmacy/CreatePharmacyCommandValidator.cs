using FluentValidation;

namespace PharmacyService.Application.Features.Pharmacies.Commands.CreatePharmacy;

public class CreatePharmacyCommandValidator
    : AbstractValidator<CreatePharmacyCommand>
{
    public CreatePharmacyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty().MaximumLength(20);

        RuleFor(x => x.BranchAddress)
            .NotEmpty().MaximumLength(500);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}