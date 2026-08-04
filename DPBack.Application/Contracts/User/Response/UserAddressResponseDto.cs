namespace DPBack.Application.Contracts.User.Response{

    public record UserAddressResponseDto(
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