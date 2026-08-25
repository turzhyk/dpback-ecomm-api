using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Options.Pricing;

public class BusinesscardPricing
{
    public decimal BasePrice { get; set; }
    public Dictionary<string, decimal> ThicknessPrices  {get;set;} = new();
    public Dictionary<string, decimal> CoatingPrices { get; set; } = new();
}