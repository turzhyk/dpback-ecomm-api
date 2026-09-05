using DPBack.Application.Contracts;
using DPBack.Application.Contracts.User.Response;
using DPBack.Domain.Models;

namespace DPBack.Application.Mappers.User;

public static class UserMapper
{
    public static UserResponse ToDto(this Domain.Models.User user)
    {
       return new UserResponse
        (
            user.Id, user.Login, user.Email, user.CreatedAt,
            user.Role, user.Adresses?.Select(a => a.ToDto()).ToList() ?? new List<UserAddressResponseDto>()
        );
    }

    public static UserAddressResponseDto ToDto(this Domain.Models.UserAddress userAddress)
    {
        return new UserAddressResponseDto
        (
            userAddress.Id,
            userAddress.Country,
            userAddress.City,
            userAddress.Street,
            userAddress.BuildingNumber,
            userAddress.ApartmentNumber,
            userAddress.PostalCode,
            userAddress.PhoneNumber,
            userAddress.Email,
            userAddress.Options
        );
    }

 
}