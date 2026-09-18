using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Contracts.Customers;

using DPBack.Application.Contracts.User.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[Route("customers")]
[ApiController]
public class CustomerController(IOrdersService ordersService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create([FromBody] CustomerCreateRequest request, CancellationToken cToken)
    {
        var result = await ordersService.CreateCustomerAsync(request, cToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<CustomersResponseDto>> GetAll(CancellationToken cToken)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("by-phone/{phone}")]
    public async Task<ActionResult<CustomerResponseDto?>> GetByPhone(string phone, CancellationToken cToken)
    {
        var result = await ordersService.GetCustomerByPhoneAsync(phone, cToken);
        return new JsonResult(result);
    }
    [Authorize]
    [HttpGet("{id}/addresses")]
    public async Task<ActionResult<List<CustomerAddressResponse>>> GetCustomerAddresses(Guid id, CancellationToken cToken)
    {
       // var CustomerId = GetCurrentCustomerId();
        var result = await  ordersService.GetAddressesByCustomerIdAsync(id, cToken);
        return Ok(result);
    }
    [Authorize]
    [HttpPost("{id}/addresses")]
    public async Task<ActionResult> AddCustomerAddress (Guid id,[FromBody] CustomerAddressCreateRequest request, CancellationToken cToken)
    {
        // var CustomerId = GetCurrentCustomerId();

        await ordersService.AddCustomerAddressAsync(id, request, cToken);
        return Ok();
    }
    [HttpPatch("addresses/{addressId}")]
    [Authorize]
    public async Task<ActionResult> ChangeCustomerAddress(Guid addressId, [FromBody] CustomerAddressCreateRequest request, CancellationToken cToken)
    {
        // var CustomerId = GetCurrentCustomerId();
        // Implement later
        return Ok();
    }
    [HttpDelete("addresses/{addressId}")]
    [Authorize]
    public async Task<ActionResult> DeleteCustomerAddress(Guid addressId, CancellationToken cToken)
    {
        // Implement later
        return Ok();
    }
}