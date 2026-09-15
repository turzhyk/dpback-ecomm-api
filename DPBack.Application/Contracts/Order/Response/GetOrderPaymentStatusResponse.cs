using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public record GetOrderPaymentStatusResponse(
    OrderPaymentStatus PaymentStatus
);