namespace DPBack.Domain.Models;

public class UserAddress
{
    public UserAddress(Guid id, Guid userId, string? country, string? city, string? street, string? buildingNumber, string? apartmentNumber, string? postalCode, string? phoneNumber, string? email, string? options)
    {
        Id = id;
        UserId = userId;
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
    public UserAddress(){}
  
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Options { get; set; }
    
}