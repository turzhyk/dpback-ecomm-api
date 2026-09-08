using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Pricing.Calculators;

public class TshirtCalculator:IPriceCalculator
{
    public OrderItemType Type => OrderItemType.Tshirt;
    public decimal CalculateUnitPrice(ProductConfig abstractConfig)
    {
        return 45m;
       
    }
}