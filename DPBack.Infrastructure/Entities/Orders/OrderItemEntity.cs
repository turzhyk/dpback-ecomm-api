
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Infrastructure.Entities.Orders;

namespace DPBack.Infrastructure.Entities;



public class OrderItemEntity
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public decimal PricePerUnit { get; set; }
    [MaxLength(100)]
    public string? Options { get; set; }
    
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
}