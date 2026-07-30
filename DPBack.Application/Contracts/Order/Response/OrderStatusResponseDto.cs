using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class OrderStatusResponseDto
{
    public OrderStatus Status { get;set; }
}