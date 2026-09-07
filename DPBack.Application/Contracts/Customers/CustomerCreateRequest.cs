using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Contracts.Customers;

public record CustomerCreateRequest(
    string Name,
    [Phone] string Phone,
    [EmailAddress] string Email,
    string? Nip,
    string? Regon,
    string? CompanyName);