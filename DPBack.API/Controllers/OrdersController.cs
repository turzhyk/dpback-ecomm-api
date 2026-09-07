using System.Security.Claims;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController(IOrdersService service) : ControllerBase
    {
        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return null;
            return Guid.Parse(userId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Worker")]
        // Use with caution (could be a lot of data)
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersAsync(CancellationToken cToken)
        {
            var result = await service.GetAllAsync(cToken);
            
            return Ok(result);
        }
        [HttpPost(("paged"))]
        [Authorize]
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersFiltered(OrdersFilteredRequestDto request,CancellationToken cToken)
        {
            var respose = await service.GetFilteredAsync(request, cToken);
            return Ok(respose);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdAsync(Guid id, CancellationToken cToken)
        {
            var userId = GetCurrentUserId();
            if (userId is not Guid user)
                throw new UnauthorizedAccessException();
            
            var result = await service.GetByIdAsync(user, id, cToken);
            return Ok(result);
        }
        [HttpPut("{id}/assigned")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> AssignOrderTo(Guid id, [FromBody] AssignOrderRequest request, CancellationToken cToken)
        {
            await service.AssignToUserAsync(id, request.AuthorLogin, cToken);
            return Ok();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin, Worker")]
        public async Task<ActionResult> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusRequest request, CancellationToken cToken)
        {
            var userId = GetCurrentUserId();

            await service.ChangeStatusAsync(id, userId.ToString(), request.Status, cToken);
            return Ok();
        }
      
        [HttpPost]
        public async Task<ActionResult<CreateOrderResponse>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cToken)
        {
            var userId = GetCurrentUserId();
            // var userId = GetCurrentUserId();
            var response = await service.CreateAsync(userId, request, cToken);
            return Ok(response);
        }

        [HttpGet("{id}/paymentStatus")]
        public async Task<ActionResult<GetOrderPaymentStatusResponse>> GetOrderPaymentStatus(Guid id, CancellationToken cToken)
        {
            // var userId = GetCurrentUserId();
            // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var status = await service.GetPaymentStatusAsync(id, cToken);
            var response = new GetOrderPaymentStatusResponse
                { PaymentStatus = status };
            return Ok(response);
        }

        [HttpPost("{id}/suspend")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> SuspendOrder(Guid id, CancellationToken cToken)
        {
            await service.SuspendOrderAsync(id, cToken);
            return Ok();
        }
    }
}