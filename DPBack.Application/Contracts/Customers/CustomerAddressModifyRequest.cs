namespace DPBack.Application.Contracts;

public record CustomerAddressModifyRequest(
    string? Country,
    string? City,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? PostalCode,
    string? PhoneNumber,
    string? Email,
    string? Options
);