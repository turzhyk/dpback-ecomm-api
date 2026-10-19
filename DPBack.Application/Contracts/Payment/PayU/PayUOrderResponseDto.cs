using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts.Payment.PayU;

public class PayUOrderResponseDto
{
    public required string OrderId { get; set; }
    [JsonPropertyName("redirectUri")]
    public required string RedirectUri { get; set; }
    public required PayUOrderResponseStatusDto Status { get; set; }
    public required string ExtOrderId { get; set; }
}

public class PayUOrderResponseStatusDto
{
    [JsonPropertyName("statusCode")]
    public required string StatusCode { get; set; }
}