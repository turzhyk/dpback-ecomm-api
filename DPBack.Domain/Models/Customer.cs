namespace DPBack.Domain.Models;

public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
    public List<CustomerAddress>? Addresses { get; set; }
    public Guid? UserId { get; set; }
    public string? Nip { get; set; }
    public string? Regon { get; set; }
    public string? CompanyName { get; set; }
}