using System.ComponentModel.DataAnnotations;
using DPBack.Application.Contracts.User.Response;

namespace DPBack.Application.Contracts.Customers;

public record CustomerResponseDto(
    Guid Id,
    string Name,
    [Phone] string Phone,
    [EmailAddress] string? Email,
    List<CustomerAddressResponse>? Addresses);