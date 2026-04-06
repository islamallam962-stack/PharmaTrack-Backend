using PharmacyService.Domain.Common;
using PharmacyService.Domain.Exceptions;

namespace PharmacyService.Domain.Entities;

public class PharmacyBranch : BaseEntity
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public bool IsMain { get; private set; }

    // FK
    public Guid PharmacyId { get; private set; }
    public Pharmacy Pharmacy { get; private set; } = default!;

    private PharmacyBranch() { }

    public static PharmacyBranch Create(
        string name,
        string address,
        string phone,
        double latitude,
        double longitude,
        bool isMain,
        Guid pharmacyId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Branch name is required.");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Branch address is required.");

        return new PharmacyBranch
        {
            Name       = name.Trim(),
            Address    = address.Trim(),
            Phone      = phone.Trim(),
            Latitude   = latitude,
            Longitude  = longitude,
            IsMain     = isMain,
            PharmacyId = pharmacyId
        };
    }

    public void Update(string name, string address, string phone,
                       double latitude, double longitude)
    {
        Name      = name.Trim();
        Address   = address.Trim();
        Phone     = phone.Trim();
        Latitude  = latitude;
        Longitude = longitude;
        SetUpdatedAt();
    }
}