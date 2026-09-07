

using DPBack.Application.Contracts;
using DPBack.Application.Contracts.Customers;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<CreateOrderResponse> CreateAsync(Guid? userId,CreateOrderRequest createOrder, CancellationToken cToken);
        Task<List<OrderResponse>> GetAllAsync(CancellationToken cToken);
        Task<PagedResponse<OrderResponse>> GetFilteredAsync(OrdersFilteredRequestDto request, CancellationToken cToken);
        Task<OrderResponse> GetByIdAsync(Guid userId,Guid orderId, CancellationToken cToken);
        Task AssignToUserAsync(Guid orderId, string author, CancellationToken cToken);
        Task SetPaymentStatusAsync(Guid orderId, OrderPaymentStatus status, CancellationToken cToken);
        Task<OrderPaymentStatus> GetPaymentStatusAsync(Guid orderId, CancellationToken cToken);

        Task ChangeStatusAsync(Guid orderId, string author, OrderStatus newStatus, CancellationToken cToken);

        Task<IEnumerable<DeliveryOptionResposeDto>> GetDeliveryOptionList();

        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateRequest request, CancellationToken cToken);
        Task<CustomersResponseDto> GetAllCustomersAsync(CancellationToken cToken);
        Task<CustomerResponseDto?> GetCustomerByPhoneAsync(string phone, CancellationToken cToken);
        Task SuspendOrderAsync(Guid id, CancellationToken cToken);
    }
}