using System.Reflection.PortableExecutable;
using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Options.Pricing;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Options;


namespace DPBack.Application.Pricing.Calculators;

public class BusinesscardCalculator : IPriceCalculator
{
    private readonly BusinesscardPricing _pricing;
    public OrderItemType Type => OrderItemType.Businesscard;
    
    public BusinesscardCalculator(IOptions<Options.Pricing.Pricing> pricing)
    {
        _pricing = pricing.Value.Businesscard;
    }

    public decimal CalculateUnitPrice(ProductConfig abstractConfig)
    {
        if (abstractConfig == null)
            throw new Exception("Invalid configurations");
        var config = abstractConfig as BusinesscardConfig    ?? throw new ArgumentException(
            $"Expected {nameof(BusinesscardConfig)}, got {abstractConfig.GetType().Name}");

        decimal price = _pricing.BasePrice;
        price += _pricing.ThicknessPrices.GetValueOrDefault(config.Thickness.ToString());
        price += _pricing.CoatingPrices.GetValueOrDefault(config.Coating.ToString());
        return price;
    }
}