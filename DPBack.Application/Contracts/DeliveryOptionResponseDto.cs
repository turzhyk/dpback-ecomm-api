namespace DPBack.Application.Contracts;

public class DeliveryOptionResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public decimal Price { get; set; }
}