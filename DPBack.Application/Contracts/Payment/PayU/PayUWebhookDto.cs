using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts.Payment.PayU;

public class PayUWebhookDto
{
    [JsonPropertyName("order")] public required PayUOrderDto Order { get; set; }
}

public class PayUOrderDto
{
    [JsonPropertyName("orderId")] public required string OrderId { get; set; }
    [JsonPropertyName("extOrderId")] public required string ExtOrderId { get; set; }
    [JsonPropertyName("status")] public required string Status { get; set; }
}