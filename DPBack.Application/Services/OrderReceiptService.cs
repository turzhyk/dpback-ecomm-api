using DPBack.Application.Abstractions;
using DPBack.Application.Exceptions;
using DPBack.Application.Options;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Services;

public class OrderReceiptService(IOrdersRepository repo, IReceiptGenerator generator, IOptions<CompanyOptions> companyOptions) : IReceiptService
{
    private readonly CompanyOptions _companyOptions = companyOptions.Value;
    public async Task<Guid> CreateReceiptTaskAsync(Guid orderId, CancellationToken cToken)
    {
        var result = await repo.CreateOrderReceiptTaskAsync(new OrderReceiptTask
        {
            Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, OrderId = orderId, Status = OrderReceiptStatus.Pending
        }, cToken);
        return result;
    }

    public async Task<byte[]> GetReceiptFileAsync(Guid orderId, CancellationToken cToken)
    {
        var order = await repo.GetById(orderId, cToken);
        if (order is null)
            throw new OrderDoesNotExistException();
        return generator.GenerateReceiptPdf(order, _companyOptions, cToken);
    }
}