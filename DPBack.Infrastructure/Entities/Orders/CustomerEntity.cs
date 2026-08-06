namespace DPBack.Infrastructure.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
    public Guid? UserId { get; set; }
}