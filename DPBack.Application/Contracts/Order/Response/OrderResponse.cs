using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts
{
    public record OrderResponse
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public List<OrderItemResponse> Items { get; set; }
        public Guid AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuspended { get; set; }
        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        public List<OrderHistoryElementResponse> History { get; set; }
    };
}