using System.Text.Json.Serialization;

namespace DPBack.Domain.Enums.Products;

public class Businesscard
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Thickness
    {
        T250,
        T300,
        T350
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Coating
    {
        Glossy,
        Matte,
        SoftTouch
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PrintType
    {
        BW,
        Color,
        None
    }
}