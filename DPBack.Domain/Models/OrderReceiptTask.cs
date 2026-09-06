using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Domain.Models;

public class OrderReceiptTask
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderReceiptStatus Status { get; set; }
}