using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class GetOrderPaymentStatusResponse
{
    public OrderPaymentStatus PaymentStatus { get; set; }
}