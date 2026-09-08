using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Contracts.Customers;
using DPBack.Application.Exceptions;
using DPBack.Application.Extensions;
using DPBack.Application.Mappers;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DPBack.Application.Services
{
    public class OrdersService(
        IOrdersRepository ordersRepo,
        IPaymentService paymentService,
        IPriceCalcService priceCalcService,
        ILogger<OrdersService> logger,
        ProductConfigMapperFactory optionsMapper)
        : IOrdersService

    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
        {
            { OrderStatus.New, [OrderStatus.InProgress, OrderStatus.Cancelled] },
            {
                OrderStatus.InProgress, [OrderStatus.Produced, OrderStatus.InProgress]
            },
            {
                OrderStatus.Produced, [OrderStatus.Packing, OrderStatus.ReadyForShipping]
            },
            {
                OrderStatus.Packing, [OrderStatus.ReadyForShipping]
            },
            { OrderStatus.ReadyForShipping, [OrderStatus.InDelivery] },
            { OrderStatus.InDelivery, [OrderStatus.Done] },
            { OrderStatus.Done, [] }
        };


        public async Task<List<OrderResponse>> GetAllAsync(CancellationToken cToken)
        {
            logger.LogInformation("Getting all orders");
            var orders = await ordersRepo.GetAll(cToken, 0, 100);
            var response = orders.Select(o =>
                o.ToDto()).ToList();
            return response;
        }

        public async Task<PagedResponse<OrderResponse>> GetFilteredAsync(OrdersFilteredRequestDto request,
            CancellationToken cToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            logger.LogInformation("Requesting {pageSize} orders for page nr. {pageNumber}",
                request.PageSize,
                request.PageNumber);

            var orders = await ordersRepo.GetAll(cToken, skip, request.PageSize);

            var totalCount = await ordersRepo.Count(cToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);
            return new PagedResponse<OrderResponse>
            {
                Items = orders.Select(o => o.ToDto()).ToList(),
                TotalItems = totalCount,
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<OrderResponse> GetByIdAsync(Guid userId, Guid orderId, CancellationToken cToken)
        {
            logger.LogInformation(
                "Getting order {orderId} for user {userId}",
                orderId,
                userId);
            var order = await ordersRepo.GetById(orderId, cToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");

            return order.ToDto();
        }

        public async Task<CreateOrderResponse> CreateAsync(Guid? userId, CreateOrderRequest request,
            CancellationToken cToken)
        {
            logger.LogInformation("Creating new order for user {userId}", userId);
            var authorId = userId ?? Guid.NewGuid();
            var customerId = request.CustomerId ?? Guid.NewGuid();
            if (request.CustomerId is null)
                throw new CustomerDoesNotExistException("customer does not exist");
            if (request.CustomerId is Guid _customerId)
            {
                var customerExists = await ordersRepo.CustomerExistsAsync(_customerId, cToken);
                if (!customerExists)
                    throw new CustomerDoesNotExistException($"customer {_customerId} does not exist");
            }

           

            var items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = i.Quantity,
                Type = i.Type,
                Options = optionsMapper.Map(i.Type, i.Options),
            }).ToList();
            decimal totalPrice = 0;
            foreach (var i in items)
            {
                var unitPrice = priceCalcService.Calculate(i);
                i.PricePerUnit = unitPrice;
                totalPrice += unitPrice*i.Quantity;
            }

            var paymentStatus = request.Paid ? OrderPaymentStatus.Paid : OrderPaymentStatus.Waiting;
            var orderId = Guid.NewGuid();
            var initHistoryElement = new OrderHistoryElement
            {
                OrderId = orderId,
                Status = OrderStatus.New,
                ChangedAt = DateTime.UtcNow,
                AuthorLogin = userId?.ToString() ?? "-",
                Id = Guid.NewGuid()
            };
            var history = new List<OrderHistoryElement> { initHistoryElement };
            var (order, error) = Order.Create(
                orderId,
                0,
                request.Desc,
                totalPrice,
                customerId,
                items,
                authorId,
                DateTime.UtcNow,
                false,
                status: OrderStatus.New,
                paymentStatus: paymentStatus,
                history
            );
            await ordersRepo.Create(order, cToken);
            if (paymentStatus == OrderPaymentStatus.Paid)
            {
                return new CreateOrderResponse(order.Id);
            }
            else
            {
                var paymentUrl = await paymentService.CreatePayment(order.Id.ToString(), totalPrice);
                return new CreateOrderResponse(order.Id, paymentUrl);
            }
        }

        public async Task ChangeStatusAsync(Guid orderId, string author, OrderStatus newStatus,
            CancellationToken cToken)
        {
            var order = await ordersRepo.GetById(orderId, cToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");
            logger.LogInformation("Changing {orderId} order status from {oldStatus} to {newStatus} by {author}",
                orderId, order.Status, newStatus, author);

            // if (order.AssignedTo != author)
            //     throw new StatusChangeNotAllowedException();


            if (AllowedTransitions[order.Status].Contains(newStatus))
            {
                var newAuthor = newStatus == OrderStatus.InProgress ? author : "";
                await ordersRepo.ChangeStatus(orderId, author, newStatus, newAuthor, cToken);
            }
            else
                throw new StatusChangeNotAllowedException();
        }

        public async Task<OrderPaymentStatus> GetPaymentStatusAsync(Guid orderId, CancellationToken cToken)
        {
            logger.LogInformation("Getting order {orderId} payment status", orderId);
            var order = await ordersRepo.GetById(orderId, cToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");
            return order.PaymentStatus;
        }

        public async Task SetPaymentStatusAsync(Guid orderId, OrderPaymentStatus status, CancellationToken cToken)
        {
            var order = await ordersRepo.GetById(orderId, cToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");
            if (order.PaymentStatus == status)
                throw new StatusChangeNotAllowedException();

            logger.LogInformation("Changing order {orderId} payment status from {oldStatus} to {newStatus}",
                orderId, order.PaymentStatus, status);
            await ordersRepo.SetPaymentStatus(orderId, status, cToken);
        }

        public async Task AssignToUserAsync(Guid orderId, string author, CancellationToken cToken)
        {
            var order = await ordersRepo.GetById(orderId, cToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderId} not found");

            await ordersRepo.AssignOrderWithStatus(
                orderId,
                author,
                new OrderHistoryElement
                {
                    Status = OrderStatus.InProgress,
                    AuthorLogin = author,
                    ChangedAt = DateTime.UtcNow
                }, cToken);
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateRequest request,
            CancellationToken cToken)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(), Name = request.Name, Phone = request.Phone, Email = request.Email,
                UserId = Guid.Empty
            };

            var result = await ordersRepo.CreateCustomerAsync(customer, cToken);
            return new CustomerResponseDto(customer.Id, customer.Name, customer.Phone, customer.Email);
        }

        public async Task<CustomersResponseDto> GetAllCustomersAsync(CancellationToken cToken)
        {
            var customers = await ordersRepo.GetAllCustomersAsync(cToken);
            var result =
                new CustomersResponseDto(customers.Select(x => new CustomerResponseDto(x.Id, x.Name, x.Phone, x.Email))
                    .ToList());
            return result;
        }

        public async Task<CustomerResponseDto?> GetCustomerByPhoneAsync(string phone, CancellationToken cToken)
        {
            var result = await ordersRepo.GetCustomerByPhoneAsync(phone, cToken);
            if (result is null)
                return null;
            return new CustomerResponseDto(result.Id, result.Name, result.Phone, result.Email);
        }

        public async Task<IEnumerable<DeliveryOptionResposeDto>> GetDeliveryOptionList() => null;

        public async Task SuspendOrderAsync(Guid id, CancellationToken cToken)
        {
            var order = await ordersRepo.GetById(id, cToken);
            if (order is null)
                throw new OrderDoesNotExistException(id);
            if (order.Status == OrderStatus.Done)
                throw new StatusChangeNotAllowedException($"Unable to change order {id} status.");
            await ordersRepo.SuspendOrderAsync(id, cToken);
        }
    }
}