using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IReceiptGenerator
{
    Task<byte[]> GenerateReceiptPdf(Order order,CancellationToken cToken);
}