using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Mappers.Config;

public class TshirtConfigMapper : IProductConfigMapper
{
    private readonly JsonSerializerOptions _jsonOptions;
    public OrderItemType Type => OrderItemType.Tshirt;

    public TshirtConfigMapper(JsonSerializerOptions jsonOptions)
    {
        _jsonOptions = jsonOptions;
    }

    public ProductConfig Map(JsonElement json)
    {
        var result = json.Deserialize<TshirtConfig>(_jsonOptions) ?? throw new JsonException("Invalid config");

        return result;
    }
}