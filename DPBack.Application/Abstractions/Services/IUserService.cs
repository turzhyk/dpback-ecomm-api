using DPBack.Application.Contracts;
using DPBack.Application.Contracts.User.Response;

namespace DPBack.Application.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUserAsync(UserCreateRequest request, CancellationToken cToken);
    Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken cToken);
    Task<UserLoginResponse> RefreshToken(string oldRefreshToken,CancellationToken cToken);
    Task<UserResponse> GetByEmailAsync(string email, CancellationToken cToken);
    Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cToken);
   
}