using System.Text.Json;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;
using Microsoft.AspNetCore.Http;

namespace DPBack.Application.Contracts;

public record OrderItemRequest(
    int Quantity,
    OrderItemType Type,
    JsonElement Options,
    string? FileKey
);