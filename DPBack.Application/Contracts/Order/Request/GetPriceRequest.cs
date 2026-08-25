using System.Text.Json;
using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public record GetPriceRequest(
    OrderItemType Type,
    JsonElement Configuration
);