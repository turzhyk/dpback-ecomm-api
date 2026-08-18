using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Abstractions;

public interface IProductConfigMapper
{
    OrderItemType Type { get; }
    ProductConfig Map(JsonElement json);
}