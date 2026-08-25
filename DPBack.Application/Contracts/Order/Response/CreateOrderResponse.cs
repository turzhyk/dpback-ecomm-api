namespace DPBack.Application.Contracts;

public record CreateOrderResponse(
    Guid OrderId,
    string? PaymentUrl = null
);