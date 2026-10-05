
using System.Security.Claims;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IUserService service) : ControllerBase
{
    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("No active user found");
        }

        return Guid.Parse(userId);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser(UserCreateRequest request, CancellationToken cToken)
    {
        var id = await service.CreateUserAsync(request, cToken);
        return Ok(id);
    }
    /// <summary>
    /// Development admin account: admin@local, 0000
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> LoginUser([FromBody] UserLoginRequest request, CancellationToken cToken)
    {
        try
        {
            var result = await service.Login(request, cToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<UserLoginResponse>> RefreshToken(string oldRefreshToken, CancellationToken cToken)
    {
        var result = await service.RefreshToken(oldRefreshToken, cToken);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id, CancellationToken cToken)
    {
        var userDto = service.GetByIdAsync(id, cToken);
        return Ok();
    }

    
}