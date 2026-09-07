using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Mappers;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Domain.Models.Products;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using DPBack.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories
{
    public class OrdersRepository(OrderStoreDbContext context, ProductConfigMapperFactory mapper)
        : IOrdersRepository

    {
        private static Order MapToOrder(OrderEntity e, ProductConfigMapperFactory mapper)
        {
            var items = e.Items.Select(i => new OrderItem
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Type = i.Type,
                Options = string.IsNullOrEmpty(i.Options)
                    ? null
                    : mapper.Map(i.Type, JsonSerializer.Deserialize<JsonElement>(i.Options)),
            }).ToList();
            var history = e.History.Select(h => new OrderHistoryElement
            {
                Id = h.Id,
                Status = h.Status,
                ChangedAt = h.ChangedAt,
                AuthorLogin = h.AuthorLogin,
                OrderId = e.Id
            }).ToList();
            var order = Order.Create(
                e.Id,
                e.OrderNumber,
                e.Descriprion,
                e.TotalPrice,
                e.CustomerId,
                items,
                new Guid(e.AssignedTo),
                e.CreatedAt,
                e.IsSuspended,
                e.Status,
                e.PaymentStatus,
                history
            ).Order;


            return order;
        }

        public async Task<Order?> GetById(Guid id, CancellationToken cToken)
        {
            var orderEntity =
                await context.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(cToken);
            if (orderEntity == null)
                return null;
            return MapToOrder(orderEntity, mapper);
        }

        public async Task<int> Count(CancellationToken cToken)
        {
            var count = await context.Orders
                .AsQueryable()
                .CountAsync(cToken);
            return count;
        }

        public async Task<List<Order>> GetAll(CancellationToken cToken, int skip = 0, int take = 20)
        {
            var orderEntities =
                await context.Orders
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(take)
                    .Include(o => o.Items)
                    .Include(o => o.History)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync(cToken);

            return orderEntities.Select(x => MapToOrder(x, mapper)).ToList();
        }

        public async Task<Guid> Create(Order order, CancellationToken cToken)
        {
           
            var items = order.Items.Select(i => new OrderItemEntity
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Type = i.Type,
                PricePerUnit = i.PricePerUnit,
                Options = i.Options is null
                    ? null
                    : JsonSerializer.Serialize(i.Options, i.Options.GetType()),
            }).ToList();
            var historyEntities = order.History.Select(x => new OrderHistoryElementEntity
            {
                Id = x.Id,
                AuthorLogin = x.AuthorLogin,
                OrderId = x.OrderId,
                Status = x.Status,
                ChangedAt = x.ChangedAt
            }).ToList();
            var orderEntity = new OrderEntity
            {
                Id = order.Id,
                Descriprion = order.Description,
                TotalPrice = order.TotalPrice,
                AssignedTo = order.AssignedTo.ToString(),
                Items = items,
                History = historyEntities,
                CreatedAt = order.CreatedAt,
                PaymentStatus = order.PaymentStatus,
                AddressSnapshot = "",
                Status = order.Status
            };
            await context.Orders.AddAsync(orderEntity, cToken);
            await context.SaveChangesAsync(cToken);
            return order.Id;
        }

        public async Task SetPaymentStatus(Guid orderId, OrderPaymentStatus status, CancellationToken cToken)
        {
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            order.PaymentStatus = status;
            await context.SaveChangesAsync();
        }

        public async Task<OrderPaymentStatus?> GetPaymentStatus(Guid orderId, CancellationToken cToken)
        {
            var order = await context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                return null;
            return order.PaymentStatus;
        }


        public async Task ChangeStatus(Guid orderId, string author, OrderStatus status, string newAuthor,
            CancellationToken cToken)
        {
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);
            if (order == null)
                throw new Exception($"No order found with id {orderId}");
            order.Status = status;

            order.AssignedTo = newAuthor;

            context.OrderStatusHistories.Add(new OrderHistoryElementEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                AuthorLogin = author,
                Status = status,
                ChangedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync(cToken);
        }

        public async Task AssignOrderWithStatus(
            Guid orderId,
            string author,
            OrderHistoryElement historyElement, CancellationToken cToken)
        {
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cToken);

            if (order == null)
                throw new Exception($"No order found with id {orderId}");

            order.AssignedTo = author;

            context.OrderStatusHistories.Add(new OrderHistoryElementEntity
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Status = historyElement.Status,
                AuthorLogin = historyElement.AuthorLogin,
                ChangedAt = historyElement.ChangedAt,
            });

            await context.SaveChangesAsync(cToken);
        }


        public async Task Update(Order order, CancellationToken cToken)
        {
            var orderEntity =
                await context.Orders
                    .Include(o => o.History)
                    .FirstOrDefaultAsync(o => o.Id == order.Id, cToken);
            if (orderEntity == null)
                throw new Exception("Order not found");
            orderEntity.History.Add(new OrderHistoryElementEntity
            {
                Order = orderEntity,
                Status = order.History.Last().Status,
                AuthorLogin = order.History.Last().AuthorLogin,
                ChangedAt = order.History.Last().ChangedAt,
            });
            await context.SaveChangesAsync(cToken);
        }

        public async Task<Guid> Update(Guid id, string description, decimal price, string assignedTo,
            CancellationToken cToken)
        {
            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(i => i
                    .SetProperty(o => o.Descriprion, o => description)
                    .SetProperty(o => o.TotalPrice, o => price)
                    .SetProperty(o => o.AssignedTo, o => assignedTo), cToken);
            return id;
        }

        public async Task<Guid> CreateCustomerAsync(Customer customer, CancellationToken cToken)
        {
            var entity = customer.ToEntity();
            var existingCustomer = await context.Customers.FirstOrDefaultAsync(x => x.Phone == entity.Phone);
            if (existingCustomer != null)
                return existingCustomer.Id;
            else
            {
                await context.Customers.AddAsync(entity, cToken);
                await context.SaveChangesAsync(cToken);
                return entity.Id;
            }
        }

        public async Task<Customer?> GetCustomerByPhoneAsync(string phone, CancellationToken cToken)
        {
            var entity = await context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone, cToken);
            if (entity is null)
                return null;
            return entity.ToModel();
        }

        public async Task<List<Customer>> GetAllCustomersAsync(CancellationToken cToken)
        {
            var entities = await context.Customers.AsNoTracking().ToListAsync(cToken);
            var result = entities.Select(x => x.ToModel()).ToList();
            return result;
        }

        public async Task<Guid> Delete(Guid id, CancellationToken cToken)
        {
            await context.Orders
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync(cToken);
            return id;
        }

        public async Task<bool> CustomerByPhoneExistsAsync(string phone, CancellationToken cToken)
        {
            return await context.Customers.AnyAsync(x => x.Phone == phone, cToken);
        }

        public async Task<bool> CustomerExistsAsync(Guid id, CancellationToken cToken)
        {
            return await context.Customers.AnyAsync(x => x.Id == id, cToken);
        }

        public async Task SuspendOrderAsync(Guid id, CancellationToken cToken)
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == id, cToken);
            if (order is not null)
            {
                order.IsSuspended = true;
                await context.SaveChangesAsync(cToken);
            }
        }

        public async Task<Guid> CreateOrderReceiptTaskAsync(OrderReceiptTask task, CancellationToken cToken)
        {
            await context.OrderReceiptTasks.AddAsync(new OrderReceiptTaskEntity
                { Id = task.Id, OrderId = task.OrderId, Status = task.Status, CreatedAt = DateTime.UtcNow }, cToken);
            await context.SaveChangesAsync(cToken);
            return task.Id;
        }

        public async Task ChangeOrderReceiptStatusAsync(Guid orderReceiptId, OrderReceiptStatus status,
            CancellationToken cToken)
        {
            await context.OrderReceiptTasks
                .Where(o => o.Id == orderReceiptId)
                .ExecuteUpdateAsync(i =>
                    i.SetProperty(x => x.Status, status), cToken);

            await context.SaveChangesAsync(cToken);
        }

        public async Task<List<Guid>> GetOrderReceiptTasksWithStatusAsync(OrderReceiptStatus status,
            CancellationToken cToken)
        {
            var entities = await context.OrderReceiptTasks
                .Where(x => x.Status == status)
                .AsNoTracking()
                .ToListAsync(cToken);
            var result = entities.Select(x => x.Id).ToList();
            return result;
        }
    }
}