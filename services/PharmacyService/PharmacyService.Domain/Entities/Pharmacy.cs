using PharmacyService.Domain.Common;
using PharmacyService.Domain.Enums;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Domain.Entities;

public class Pharmacy : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string LicenseNumber { get; private set; } = default!;
    public string OwnerName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public PharmacyStatus Status { get; private set; } = PharmacyStatus.PendingApproval;

    // اليوزر اللي سجّل الصيدلية من Identity Service
    public Guid OwnerId { get; private set; }

    private readonly List<PharmacyBranch> _branches = new();
    public IReadOnlyCollection<PharmacyBranch> Branches => _branches.AsReadOnly();

    private Pharmacy() { }

    public static Pharmacy Create(
        string name,
        string licenseNumber,
        string ownerName,
        string email,
        string phone,
        Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Pharmacy name is required.");

        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new DomainException("License number is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        return new Pharmacy
        {
            Name          = name.Trim(),
            LicenseNumber = licenseNumber.Trim().ToUpper(),
            OwnerName     = ownerName.Trim(),
            Email         = email.Trim().ToLower(),
            Phone         = phone.Trim(),
            OwnerId       = ownerId
        };
    }

    public void Update(string name, string ownerName,
                       string email, string phone)
    {
        Name      = name.Trim();
        OwnerName = ownerName.Trim();
        Email     = email.Trim().ToLower();
        Phone     = phone.Trim();
        SetUpdatedAt();
    }

    public void Activate()
    {
        if (Status == PharmacyStatus.Active)
            throw new DomainException("Pharmacy is already active.");

        Status = PharmacyStatus.Active;
        SetUpdatedAt();
    }

    public void Suspend()
    {
        if (Status == PharmacyStatus.Suspended)
            throw new DomainException("Pharmacy is already suspended.");

        Status = PharmacyStatus.Suspended;
        SetUpdatedAt();
    }

    public void AddBranch(PharmacyBranch branch)
    {
        if (_branches.Any(b => b.Name == branch.Name))
            throw new DomainException("Branch with same name already exists.");

        _branches.Add(branch);
    }
}