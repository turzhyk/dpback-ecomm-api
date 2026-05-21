using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Contracts
{
    public record UserAddressCreateDto
    {
        public required string Country { get; init; }
        public required string City { get; init; }
        public required string Street { get; init; }
        public required string BuildingNumber { get; init; }
        public string? ApartmentNumber { get; init; }
        public required string PostalCode { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Email { get; init; }
        public required string? Options { get; init; }
    }
}