namespace DPBack.Application.Contracts;

public record CreateOrderDraftRequest(Guid IdempotencyKey, Guid CustomerId);