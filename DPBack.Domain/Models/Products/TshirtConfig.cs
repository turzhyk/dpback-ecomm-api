using System.Text.Json.Serialization;
using DPBack.Domain.Enums.Products;

namespace DPBack.Domain.Models.Products;

public class TshirtConfig : ProductConfig
{
    [JsonPropertyName("size")] public Clothes.Size Size { get; set; }
    [JsonPropertyName("color")] public Clothes.Color Color { get; set; }

    [JsonPropertyName("material")] public Clothes.Material Material { get; set; }

    [JsonPropertyName("print")] public Clothes.PrintType PrintType { get; set; }
}