using DPBack.Application.Contracts.User.Response;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public record UserResponse(
    Guid Id,
    string? Login,
    string? Email,
    DateTime CreatedAt,
    UserRole Role,
    IReadOnlyList<UserAddressResponseDto>? Addresses
);