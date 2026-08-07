using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Contracts.Customers;

public record CustomerResponseDto(Guid Id, string Name, [Phone] string Phone, [EmailAddress] string Email);