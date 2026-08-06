using DPBack.Domain.Models;
using DPBack.Infrastructure.Entities;

namespace DPBack.Infrastructure.Mappers;

public static class UserMappers
{
    public static User ToModel(this UserEntity e)
    {
        return new User(e.Id, e.Login, e.PasswordHash, e.Email, e.Role, e.CreatedAt);
    }

    public static UserAddress ToModel(this UserAdressEntity e)
    {
        return new UserAddress(e.Id, e.UserId, e.Country, e.City, e.Street, e.BuildingNumber, e.ApartmentNumber,
            e.PostalCode, e.PhoneNumber, e.Email, e.Options);
    }

    public static CustomerEntity ToEntity(this Customer c)
    {
        return new CustomerEntity { Id = c.Id, Phone = c.Phone, Name = c.Name, Email = c.Email, UserId = c.UserId };
    }
    public static Customer ToModel(this CustomerEntity c)
    {
        return new Customer { Id = c.Id, Phone = c.Phone, Name = c.Name, Email = c.Email, UserId = c.UserId };
    }
}