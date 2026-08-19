using System.Text.Json.Serialization;

namespace DPBack.Domain.Enums.Products;

public class Clothes
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Size
    {
        XS, S, M, L, XL, XXL
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Color
    {
        White, Black, DarkGrey, LightGrey
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Material
    {
        Cotton, Polyester
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PrintType
    {
        Sublimation, Transfer
    }
}