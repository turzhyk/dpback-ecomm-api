namespace DPBack.Application.Abstractions;

public interface IReceiptService
{
    Task<Guid> CreateReceiptTaskAsync(Guid orderId, CancellationToken cToken);
    Task<(byte[] pdf,string fileName, string email)> GetReceiptFileAsync(Guid orderId, CancellationToken cToken);
}