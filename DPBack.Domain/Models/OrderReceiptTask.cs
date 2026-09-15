using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Domain.Models;

public class OrderReceiptTask
{
    public Guid Id { get; set; }
    public required Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public OrderReceiptStatus Status { get; set; }
}