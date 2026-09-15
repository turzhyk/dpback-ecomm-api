using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Options.Pricing;

public class BusinesscardPricing
{
    public required decimal BasePrice { get; set; }
    public required Dictionary<string, decimal> ThicknessPrices  {get;set;} = new();
    public required Dictionary<string, decimal> CoatingPrices { get; set; } = new();
}