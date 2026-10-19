using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Products;
using DPBack.Domain.Enums;
using DPBack.Domain.Enums.Products;

namespace DPBack.Application.Services;

public class ProductsService : IProductsService
{
    public ProductListResponse ListProducts()
    {
        return new ProductListResponse
        {
            Products =
            [
                new ProductSchemeDto
                {
                    Name = "Business cards",
                    Key = OrderItemType.Businesscard,
                    Options =
                    [
                        new ProductOptionSchemeDto
                        {
                            Name = "thickness",
                            Key = nameof(Businesscard.Thickness),
                            Values = Enum.GetNames<Businesscard.Thickness>().ToList()
                        },
                        new ProductOptionSchemeDto
                        {
                            Name = "coating",
                            Key = nameof(Businesscard.Coating),
                            Values = Enum.GetNames<Businesscard.Coating>().ToList()
                        }
                    ]
                },
                new ProductSchemeDto
                {
                    Name = "T Shirt",
                    Key = OrderItemType.Tshirt,
                    Options =
                    [
                        new ProductOptionSchemeDto
                        {
                            Name = "material",
                            Key = nameof(Clothes.Material),
                            Values = Enum.GetNames<Clothes.Material>().ToList()
                        },
                        new ProductOptionSchemeDto
                        {
                            Name = "color",
                            Key = nameof(Clothes.Color),
                            Values = Enum.GetNames<Clothes.Color>().ToList()
                        },
                        new ProductOptionSchemeDto
                        {
                            Name = "print type",
                            Key = nameof(Clothes.PrintType),
                            Values = Enum.GetNames<Clothes.PrintType>().ToList()
                        },
                        new ProductOptionSchemeDto
                        {
                            Name = nameof(Clothes.Size),
                            Key = nameof(Clothes.Size),
                            Values = Enum.GetNames<Clothes.Size>().ToList()
                        }
                    ]
                }
            ]
        };
    }

 
}