using PharmacyService.Application.DTOs;

namespace PharmacyService.Application.DTOs;

public record PharmacyDto(
    Guid                     Id,
    string                   Name,
    string                   LicenseNumber,
    string                   OwnerName,
    string                   Email,
    string                   Phone,
    string                   Status,
    DateTime                 CreatedAt,
    List<PharmacyBranchDto>  Branches
);