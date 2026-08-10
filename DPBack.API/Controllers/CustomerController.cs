using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;
[Route("customers")]
[ApiController]
public class CustomerController:ControllerBase
{
    private readonly IOrdersService _ordersService;

    public CustomerController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CustomerCreateRequest request, CancellationToken cToken)
    {
        var result = await _ordersService.CreateCustomerAsync(request, cToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomersResponseDto>> GetAll(CancellationToken cToken)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("/by-phone/{phone}")]
    public async Task<ActionResult<CustomerResponseDto?>> GetByPhone(string phone, CancellationToken cToken)
    {
        var result = await _ordersService.GetCustomerByPhoneAsync(phone, cToken);
        return Ok(result);
    }
}