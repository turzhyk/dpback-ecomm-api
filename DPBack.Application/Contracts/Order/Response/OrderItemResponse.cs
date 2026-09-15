using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;


public record OrderItemResponse
{
    public int Quantity { get; set; }
    public OrderItemType Type { get; set; }
    public decimal PricePerUnit { get; set; }
    public JsonElement? Options { get; set; }
}