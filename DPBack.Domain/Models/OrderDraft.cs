namespace DPBack.Domain.Models;

public class OrderDraft
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid IdempotencyKey { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsUsed { get; set; } = false;
}