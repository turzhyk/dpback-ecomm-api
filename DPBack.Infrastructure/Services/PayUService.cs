using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DPBack.Infrastructure.Payments;

public class PayUService(
    IPaymentTokenProvider tokenProvider,
    IPaymentObjectRepository paymentRepo,
    IHttpClientFactory clientFactory,
    IOptions<PayUOptions> options,
    ILogger<PayUService> logger)
    : IPaymentService
{
    private readonly PayUOptions _options = options.Value;
    private readonly ILogger<PayUService> _logger = logger;

    public async Task<string> CreatePaymentAsync(string orderId, decimal totalPrice, CancellationToken cToken)
    {
        var token = await tokenProvider.GetToken();
        if (token == null)
            throw new Exception("payu token is null");
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false 
        };

        var client = clientFactory.CreateClient("PayU");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        string notifyUrl = _options.NotifyUrl + "/api/payments/notify";
        var payuOrder = new
        {
            continueUrl = $"http://localhost:3000/payment/{orderId}/payment",
            notifyUrl = notifyUrl,
            customerIp = "127.0.0.1",
            merchantPosId = _options.ClientId,
            description = "test order",
            currencyCode = "PLN",
            totalAmount =  Convert.ToInt32(totalPrice * 100m),
            extOrderId = orderId,
            products = new[]
            {
                new { name = "Order", unitPrice =  Convert.ToInt32(totalPrice * 100m), quantity = "1" }
            }
        };
        Console.WriteLine(payuOrder);
        Console.WriteLine($"Token : {token}");
        var response = await client.PostAsJsonAsync("/api/v2_1/orders", payuOrder);

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"RAW RESPONSE: {content}");

        var result = JsonSerializer.Deserialize<PayUOrderResponseDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result == null || string.IsNullOrEmpty(result.RedirectUri))
            throw new PayUException("RedirectUri not found in PayU response");

        var payment = new PaymentObject
        {
            Id = Guid.NewGuid(), OrderId = new Guid(orderId), CustomerId = Guid.NewGuid(),
            PaymentLink = result.RedirectUri, PaymentId = result.OrderId, Status = OrderPaymentStatus.Waiting
        };
        await paymentRepo.AddPaymentAsync(payment, cToken);
        await paymentRepo.SaveChangesAsync(cToken);
        return result.RedirectUri;
    }

    public async Task CapturePayment(string orderId)
    {
        var token = await tokenProvider.GetToken();
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false 
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://secure.snd.payu.com")
        };
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsync($"/api/v2_1/orders/{orderId}/captures",null);
        
    }
}