namespace DPBack.Application.Contracts.Products;

public record ProductListResponse
{
    public required List<ProductSchemeDto> Products { get; init; }
}

public record ProductSchemeDto
{
    public required string Name { get; init; }
    public List<ProductOptionSchemeDto>? Options { get; init; } = [];
}

public record ProductOptionSchemeDto
{
    public required string Name { get; init; }
    public required string Key { get; init; }
    public required List<string> Values { get; init; }
}