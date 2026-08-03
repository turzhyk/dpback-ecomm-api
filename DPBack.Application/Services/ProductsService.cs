using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Products;
using DPBack.Domain.Enums;

namespace DPBack.Application.Services;

public class ProductsService: IProductsService
{
    public ProductListResponce ListProducts()
    {
        var result = new ProductListResponce();
        result.Products = Enum.GetValues<OrderItemType>()
            .ToDictionary(
                x => (int)x,
                x => x.ToString());
        return  result;
    }
}