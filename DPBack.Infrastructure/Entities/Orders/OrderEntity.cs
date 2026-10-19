using System.ComponentModel.DataAnnotations;
using DPBack.Domain.Enums;

namespace DPBack.Infrastructure.Entities.Orders

{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public int OrderNumber { get; set; }
        [MaxLength(100)]
        public string? Descriprion { get; set; }
        public bool IsSuspended { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        [MaxLength(100)]
        public required string AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        [MaxLength(300)]
        public string? AddressSnapshot { get; set; }
        public Guid CustomerId { get; set; }
        
        public List<OrderItemEntity> Items { get; set; } =new (); 

        public List<OrderHistoryElementEntity> History { get; set; } = new();
        public uint  RowVersion { get; set; }
    }
}
