using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Domain.Models.Products;

namespace DPBack.Application.Abstractions;

public interface IPriceCalculator
{
    public OrderItemType Type { get; }
    public decimal Calculate(ProductConfig abstractConfig);
}