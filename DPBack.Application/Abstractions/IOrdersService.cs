

using DPBack.Application.Contracts;
using DPBack.Application.Contracts.Customers;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions
{
    public interface IOrdersService
    {
        Task<CreateOrderResponse> CreateOrder(Guid userId,CreateOrderRequest createOrder, CancellationToken cToken);
        Task<List<OrderResponse>> GetAllOrders(CancellationToken cToken);
        Task<PagedRespose<OrderResponse>> GetOrdersFiltered(OrdersFilteredRequestDto request, CancellationToken cToken);
        Task<OrderResponse> GetOrderById(Guid userId,Guid orderId, CancellationToken cToken);
        Task AssignToAsync(Guid orderId, string author, CancellationToken cToken);
        Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken);
        Task<OrderPaymentStatus> GetPaymentStatus(Guid orderId, CancellationToken cToken);

        Task ChangeStatus(Guid orderId, string author, OrderStatus newStatus, CancellationToken cToken);

        Task<IEnumerable<DeliveryOptionResposeDto>> GetDeliveryOptionList();

        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateRequest request, CancellationToken cToken);
        Task<CustomersResponseDto> GetAllCustomers(CancellationToken cToken);
        Task<CustomerResponseDto?> GetCustomerByPhoneAsync(string phone, CancellationToken cToken);
    }
}