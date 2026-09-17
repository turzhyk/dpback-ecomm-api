using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Mappers;
using DPBack.Application.Pricing;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;

public class PriceCalcService(PriceCalculatorStrategy strategy, IProductConfigMapperResolver mapperResolver)
    : IPriceCalcService
{
    public decimal Calculate(OrderItemRequest request)
    {
        var config = mapperResolver.Map(request.Type, request.Options);
        if (config is null)
            return 1;
        // throw new ArgumentException("invalid product config");
        var calculator = strategy.Get(request.Type);
        var result = calculator.CalculateUnitPrice(config);
        return result;
    }
    public decimal Calculate(OrderItem item)
    {
        if (item.Options is null)
            return 0;
        var calculator = strategy.Get(item.Type);
        var result = calculator.CalculateUnitPrice(item.Options);
        return result;
    }
}