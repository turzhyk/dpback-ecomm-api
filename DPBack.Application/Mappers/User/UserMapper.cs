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
            user.Role
        );
    }

    public static CustomerAddressResponse ToDto(this Domain.Models.CustomerAddress customerAddress)
    {
        return new CustomerAddressResponse
        (
            customerAddress.Id,
            customerAddress.Country,
            customerAddress.City,
            customerAddress.Street,
            customerAddress.BuildingNumber,
            customerAddress.ApartmentNumber,
            customerAddress.PostalCode,
            customerAddress.PhoneNumber,
            customerAddress.Email,
            customerAddress.Options
        );
    }

 
}