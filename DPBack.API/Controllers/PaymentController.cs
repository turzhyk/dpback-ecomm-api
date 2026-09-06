using System.Text.Json;
using System.Threading.Channels;
using DPBack.Application.Contracts;
using DPBack.Application.Features;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DPBack.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController(
    IOptions<PayUOptions> options,
    IOrdersService ordersService,
    IPaymentService paymentService,
    IReceiptService receiptService, ChannelWriter<Guid> channelWriter)
    : Controller
{
    private readonly PayUOptions _options = options.Value;

    [HttpPost("notify")]
    public async Task<IActionResult> Notify(CancellationToken cToken)
    {
        Request.Body.Position = 0;
        using var reader = new StreamReader(Request.Body, leaveOpen: true);
        var rawBody = await reader.ReadToEndAsync(cToken);
        Request.Body.Position = 0;

        var signatureHeader = Request.Headers["OpenPayu-Signature"];
        if (string.IsNullOrEmpty(signatureHeader))
            return BadRequest();


        if (!SignatureVerificator.Verify(rawBody, signatureHeader, _options.SecondKey))
            return Unauthorized();

        var dto = JsonSerializer.Deserialize<PayUWebhookDto>(rawBody);
        if (dto == null)
            return BadRequest("invalid notify data");

        var orderId = dto.Order.ExtOrderId;
        var payuOrderId = dto.Order.OrderId;
        var status = dto.Order.Status;

        switch (status)
        {
            case ("WAITING_FOR_CONFIRMATION"):
                var currentStatus = await ordersService.GetPaymentStatusAsync(new Guid(orderId), cToken);
                if (currentStatus == OrderPaymentStatus.Waiting)
                    await paymentService.CapturePayment(payuOrderId);
                break;
            case "CANCELED":
                await ordersService.SetPaymentStatusAsync(new Guid(orderId), OrderPaymentStatus.Cancelled, cToken);
                break;
            case "COMPLETED":
                await ordersService.SetPaymentStatusAsync(new Guid(orderId), OrderPaymentStatus.Paid, cToken);
                var taskId = await receiptService.CreateReceiptTaskAsync(new Guid(orderId), cToken);
                await channelWriter.WriteAsync(taskId, cToken);
                break;
        }

        return Ok();
    }
}