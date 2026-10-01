using DPBack.Domain.Enums;

namespace DPBack.Domain.Models;

public class PaymentObject
{
    public required Guid Id { get; set; }
    public required Guid OrderId { get; set; }
    // public Order? Order { get; init; }
    public required string PaymentLink { get; init; }
    public required string PaymentId { get; set; }
    public required OrderPaymentStatus Status { get; set; }
    public required Guid CustomerId { get; set; }
    // public Customer? Customer { get; init; }
}