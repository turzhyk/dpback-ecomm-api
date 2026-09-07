using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IReceiptGenerator
{
    byte[] GenerateReceiptPdf(Order order,CancellationToken cToken);
}