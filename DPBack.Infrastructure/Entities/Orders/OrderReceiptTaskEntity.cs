using DPBack.Domain.Enums;

namespace DPBack.Infrastructure.Entities;

public class OrderReceiptTaskEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderReceiptStatus Status { get; set; }
}