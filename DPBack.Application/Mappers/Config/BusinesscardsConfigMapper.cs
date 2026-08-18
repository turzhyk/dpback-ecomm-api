using System.Text.Json;
using System.Text.Json.Serialization;
using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Mappers.Config;

public class BusinesscardsConfigMapper : IProductConfigMapper
{
    private readonly JsonSerializerOptions _jsonOptions;

    public BusinesscardsConfigMapper(JsonSerializerOptions jsonOptions)
    {
        _jsonOptions = jsonOptions;
    }


    public OrderItemType Type => OrderItemType.Businesscard;

    public ProductConfig Map(JsonElement json)
    {
        var result = json.Deserialize<BusinesscardConfig>(_jsonOptions) ?? throw new JsonException("Invalid config");

        return result;
    }
}