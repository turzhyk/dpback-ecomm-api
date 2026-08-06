using DPBack.Application.Contracts.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[Route("customers")]
[ApiController]
public class CustomerController:ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateCustomer([FromBody] CustomerCreateRequest request, CancellationToken cToken)
    {
        return Ok();
    }
}