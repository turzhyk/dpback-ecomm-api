using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public class OrderStatusResponse
{
    public OrderStatus Status { get;set; }
}