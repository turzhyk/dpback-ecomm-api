using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts
{
    public class OrderResponse
    {
        public Guid id { get; set; }
        public int OrderNumber { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public IReadOnlyCollection<OrderItemResponse> Items { get; set; }
        public string AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuspended { get; set; }
        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        public IReadOnlyCollection<OrderHistoryElementResponse> History { get; set; }
    };
}