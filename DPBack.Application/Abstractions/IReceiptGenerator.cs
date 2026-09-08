using DPBack.Application.Options;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IReceiptGenerator
{
    byte[] GenerateReceiptPdf(Order order, CompanyOptions companyOptions,CancellationToken cToken);
}