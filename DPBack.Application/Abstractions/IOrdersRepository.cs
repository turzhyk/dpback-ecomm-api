using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersRepository
    {
        Task<Guid> Create(Order order, CancellationToken cToken);
        Task<Guid> Delete(Guid id, CancellationToken cToken);
        Task<List<Order>> GetAll(CancellationToken cToken, int skip, int take);
        Task<int> Count(CancellationToken cToken);
        Task<Order?> GetById(Guid id, CancellationToken cToken);
        Task Update(Order order, CancellationToken cToken);
        Task<Guid> Update(Guid id, string description, decimal price, string assignedTo, CancellationToken cToken);
        Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor, CancellationToken cToken);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken);
        Task<OrderPaymentStatus?> GetPaymentStatus(Guid orderId, CancellationToken cToken);

        Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement, CancellationToken cToken);

        Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cToken);
        Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<Customer?> GetCustomerByPhoneAsync(string phone, CancellationToken cToken);
        Task<bool> CustomerByPhoneExistsAsync(string phone, CancellationToken cToken);
        Task<bool> CustomerExistsAsync(Guid id, CancellationToken cToken);
        Task SuspendOrderAsync(Guid id, CancellationToken cToken);
        
        Task<Guid> CreateOrderReceiptTaskAsync(OrderReceiptTask orderReceiptTask, CancellationToken cToken);
        Task ChangeOrderReceiptStatusAsync(Guid orderReceiptId, OrderReceiptStatus status, CancellationToken cToken);
        Task<List<Guid>> GetOrderReceiptTasksWithStatusAsync(OrderReceiptStatus status, CancellationToken cToken);
    }
    
}