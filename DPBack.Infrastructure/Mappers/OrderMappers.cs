using DPBack.Domain.Models;
using DPBack.Infrastructure.Entities;

namespace DPBack.Infrastructure.Mappers;

public static class OrderMappers
{
    public static OrderHistoryElementEntity ToEntity(this OrderHistoryElement e)
    {
        return new OrderHistoryElementEntity
        {
            Id = e.Id,
            OrderId = e.OrderId,
            AuthorLogin = e.AuthorLogin,
            Message = e.Message,
            ChangedAt = e.ChangedAt,
            Status = e.Status
        };
    }

    public static OrderHistoryElement ToModel(this OrderHistoryElementEntity e)
    {
        return new OrderHistoryElement
        {
            Id = e.Id,
            OrderId = e.OrderId,
            AuthorLogin = e.AuthorLogin,
            Message = e.Message,
            ChangedAt = e.ChangedAt,
            Status = e.Status
        };
    }
}