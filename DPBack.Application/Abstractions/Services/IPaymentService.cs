namespace DPBack.Application.Abstractions;

public interface IPaymentService
{
    Task<string> CreatePaymentAsync(string token, decimal totalPrice, CancellationToken cToken);
    Task CapturePayment(string orderId);
}