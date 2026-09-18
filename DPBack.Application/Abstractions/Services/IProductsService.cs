using DPBack.Application.Contracts.Products;

namespace DPBack.Application.Abstractions;

public interface IProductsService
{
    public ProductListResponse ListProducts();
}