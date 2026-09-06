using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[ApiController]
[Route("/api/price")]
public class PriceController(IPriceCalcService priceCalcService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<decimal>> GetPricePerUnit([FromBody] OrderItemRequest request)
    {
        return priceCalcService.Calculate(request);
    }
    
}