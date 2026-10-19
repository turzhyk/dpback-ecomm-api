using System.ComponentModel.DataAnnotations;

namespace DPBack.Infrastructure.Entities.Products;

public class DeliveryTypeEntity
{
    public Guid Id { get; set; }
    [MaxLength(30)]
    public required string Title { get; set; }
    public required decimal Price { get; set; }
}