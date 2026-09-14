using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Abstractions;

public interface IProductConfigMapperResolver
{
    ProductConfig? Map(OrderItemType type, JsonElement options);
    IProductConfigMapper? Get(OrderItemType type);
}