using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Pricing.Calculators;

public class TestCalculator(IOptions<TestPricing> options):IPriceCalculator
{

    private readonly TestPricing _options = options.Value;
    public OrderItemType Type => OrderItemType.Test;
    public decimal CalculateUnitPrice(ProductConfig? abstractConfig)
    {
        return _options.BasePrice;
    }
}