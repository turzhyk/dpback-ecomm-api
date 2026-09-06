using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;

public class OrderReceiptService(IOrdersRepository repo) : IReceiptService
{
    public async Task<Guid> CreateReceiptTaskAsync(Guid orderId, CancellationToken cToken)
    {
        var result = await repo.CreateOrderReceiptTaskAsync(new OrderReceiptTask
        {
            Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, OrderId = orderId, Status = OrderReceiptStatus.Pending
        }, cToken);
        return result;
    }
}