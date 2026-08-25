using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Mappers;
using DPBack.Application.Pricing;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;


public class PriceCalcService : IPriceCalcService
{
    private readonly PriceCalculatorFactory _factory;
    private readonly ProductConfigMapperFactory _mapperFactory;

    public PriceCalcService(PriceCalculatorFactory factory, ProductConfigMapperFactory mapperFactory)
    {
        _factory = factory;
        _mapperFactory = mapperFactory;
    }

    public decimal Calculate(OrderItemRequest request)
    {

        var config = _mapperFactory.Map(request.Type, request.Options);
        if (config is null)
            throw new ArgumentException("invalid product config");
        var calculator = _factory.Get(request.Type);
        var result = calculator.Calculate(config);
        return result;
    }
}