using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Products;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[ApiController]
[Route("/products")]
public class ProductsController:ControllerBase

{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }
    [HttpGet]
    public ActionResult<ProductListResponse> GetProducts()
    {
        return _productsService.ListProducts();
    }
    
}