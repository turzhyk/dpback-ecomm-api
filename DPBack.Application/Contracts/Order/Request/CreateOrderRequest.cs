using DPBack.Application.Contracts;

namespace DPBack.Application.Contracts
{
    public record CreateOrderRequest(
        string Desc,
        Guid CreatedBy,
        IReadOnlyCollection<OrderItemRequest> Items,
        bool Paid,
        Guid? CustomerId
    );
}