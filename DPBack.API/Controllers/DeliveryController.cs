using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[Route("api/delivery")]
[ApiController]
public class DeliveryController(IOrdersService ordersService) : ControllerBase
{
    [HttpGet("list")]
    public async Task<ActionResult<List<DeliveryOptionResposeDto>>> GetList()
    {
        var result = await ordersService.GetDeliveryOptionList();
        return Ok(result);
    }
    
}