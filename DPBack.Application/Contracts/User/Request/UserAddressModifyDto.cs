namespace DPBack.Application.Contracts;

public record UserAddressModifyDto
{
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? BuildingNumber { get; init; }
    public string? ApartmentNumber { get; init; }
    public string? PostalCode { get; init; }
    public  string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public string? Options { get; init; }
};