namespace PharmacyService.Application.DTOs;

public record PharmacyBranchDto(
    Guid   Id,
    string Name,
    string Address,
    string Phone,
    double Latitude,
    double Longitude,
    bool   IsMain
);