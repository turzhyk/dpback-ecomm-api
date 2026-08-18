using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Domain.Models.Products;


namespace DPBack.Application.Mappers;

public class ProductConfigMapperFactory
{
    private readonly Dictionary<OrderItemType, IProductConfigMapper> _mappers;

    public ProductConfigMapperFactory(IEnumerable<IProductConfigMapper> mappers)
    {
        _mappers = mappers.ToDictionary(x => x.Type);
    }
    public ProductConfig? Map (OrderItemType type, JsonElement options)
    {
        var mapper = Get(type);
        if (mapper is null)
            return null;
        return mapper.Map(options);
    }
    public IProductConfigMapper? Get(OrderItemType type)
    {
        return _mappers.GetValueOrDefault(type);
    }
}