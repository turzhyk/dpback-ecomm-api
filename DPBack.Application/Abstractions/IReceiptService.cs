namespace DPBack.Application.Abstractions;

public interface IReceiptService
{
    Task<Guid> CreateReceiptTaskAsync(Guid orderId, CancellationToken cToken);
}