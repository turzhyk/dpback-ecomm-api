using System.ComponentModel.DataAnnotations;

namespace DPBack.Infrastructure.Entities.Orders;

public class CustomerAddressEntity
{
    public CustomerAddressEntity(Guid id, Guid customerId, string? country, string? city, string? street, string? buildingNumber, string? apartmentNumber, string? postalCode, string? phoneNumber, string? email, string? options)
    {
        Id = id;
        CustomerId = customerId;
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        Email = email;
        Options = options;
    }

    public Guid Id { get; set; }
  
    public Guid CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    [MaxLength(50)]
    public string? Country { get; set; }
    [MaxLength(50)]
    public string? City { get; set; }
    [MaxLength(50)]
    public string? Street { get; set; }
    [MaxLength(20)]
    public string? BuildingNumber { get; set; }
    [MaxLength(20)]
    public string? ApartmentNumber { get; set; }
    [MaxLength(20)]
    public string? PostalCode { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [MaxLength(50)]
    public string? Email { get; set; }
    [MaxLength(100)]
    public string? Options { get; set; }
    
}