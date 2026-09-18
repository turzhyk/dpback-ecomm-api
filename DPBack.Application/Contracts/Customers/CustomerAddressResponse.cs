namespace DPBack.Application.Contracts.User.Response{

    public record CustomerAddressResponse(
        Guid Id,
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
}