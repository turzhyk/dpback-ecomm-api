using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Mappers;
using DPBack.Application.Pricing;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;

public class PriceCalcService(PriceCalculatorFactory factory, ProductConfigMapperFactory mapperFactory)
    : IPriceCalcService
{
    public decimal Calculate(OrderItemRequest request)
    {
        var config = mapperFactory.Map(request.Type, request.Options);
        if (config is null)
            return 1;
        // throw new ArgumentException("invalid product config");
        var calculator = factory.Get(request.Type);
        var result = calculator.Calculate(config);
        return result;
    }
}