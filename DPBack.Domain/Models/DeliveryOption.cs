namespace DPBack.Domain.Models;

public class DeliveryOption
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required decimal Price { get; set; }
}