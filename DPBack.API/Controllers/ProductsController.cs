using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Products;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[ApiController]
[Route("/products")]
public class ProductsController(IProductsService productsService) : ControllerBase

{
    [HttpGet]
    public ActionResult<ProductListResponse> GetProducts()
    {
        return productsService.ListProducts();
    }
    
}