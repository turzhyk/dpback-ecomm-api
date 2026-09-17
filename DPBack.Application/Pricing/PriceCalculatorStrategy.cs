using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;

namespace DPBack.Application.Pricing;

public class PriceCalculatorStrategy
{
    private readonly Dictionary<OrderItemType, IPriceCalculator> _calculators;

    public PriceCalculatorStrategy(IEnumerable<IPriceCalculator> calculators)
    {
        _calculators = calculators.ToDictionary(i => i.Type);
    }

    public IPriceCalculator Get(OrderItemType type)
    {
        if (!_calculators.TryGetValue(type, out var calculator))
            throw new Exception($"No calculator for type {type}");
        return _calculators[type];
    }
}