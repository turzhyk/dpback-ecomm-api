using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[ApiController]
[Route("/api/price")]
public class PriceController(IPriceCalcService priceCalcService) : ControllerBase
{
    [HttpGet]
    public ActionResult<decimal> GetPricePerUnit([FromQuery] OrderItemRequest request)
    {
        return priceCalcService.Calculate(request);
    }
    
}