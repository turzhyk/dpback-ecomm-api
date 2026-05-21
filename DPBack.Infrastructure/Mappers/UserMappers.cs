using DPBack.Domain.Models;
using DPBack.Infrastructure.Entities;

namespace DPBack.Infrastructure.Mappers;

public static class UserMappers
{
    public static UserAddress ToModel(this UserAdressEntity e)
    {
        return new UserAddress(e.Id, e.UserId, e.Country, e.City, e.Street, e.BuildingNumber, e.ApartmentNumber,
            e.PostalCode, e.PhoneNumber, e.Email, e.Options);
    }
}