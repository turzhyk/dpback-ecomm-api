using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;
//<summary>
// Note: the interface/repository adopts a Pragmatic Clean Architecture
//</summary>
public interface IPaymentObjectRepository
{
    Task<PaymentObject?> GetPaymentByOrderAsync(Guid orderId, CancellationToken cToken);
    Task<PaymentObject?> GetPaymentByIdAsync(Guid id, CancellationToken cToken);
    Task AddPaymentAsync(PaymentObject payment, CancellationToken cToken);
    Task UpdateAsync(PaymentObject payment, CancellationToken cToken);
    Task DeletePaymentAsync(Guid id,  CancellationToken cToken);
    Task SaveChangesAsync(CancellationToken cToken);
}