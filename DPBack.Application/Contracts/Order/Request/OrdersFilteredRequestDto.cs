using DPBack.Domain.Enums;
using DPBack.Domain.Models;

namespace DPBack.Application.Contracts;

public record OrdersFilteredRequestDto(
    List<OrderStatus>? Status,
    int PageNumber = 1,
    int PageSize = 10
);