using DPBack.Application.Contracts;

namespace DPBack.Application.Contracts
{
    public record CreateOrderRequest(
        string Desc,
        IReadOnlyCollection<OrderItemRequest> Items,
        Guid IdempotencyKey,
        bool Paid,
        Guid CustomerId
    );
}