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

    public async   Task<(byte[] pdf,string fileName, string email)> GetReceiptFileAsync(Guid orderId,  CancellationToken cToken)
    {
        var order = await repo.GetById(orderId, cToken);
        if(order is null)
            throw new KeyNotFoundException("Order not found");
        var customer = await repo.GetCustomerAsync(order.CustomerId, cToken);
        if(customer is null || string.IsNullOrEmpty(customer.Email))
            throw new KeyNotFoundException("Customer not found");
        if (order is null)
            throw new OrderDoesNotExistException();
        string fileName = $"receipt_for_order_{order.Id}";
        return (generator.GenerateReceiptPdf(order, _companyOptions, cToken),fileName, customer.Email);
    }
}