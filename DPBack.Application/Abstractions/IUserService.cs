using DPBack.Application.Contracts;
using DPBack.Application.Contracts.User.Response;

namespace DPBack.Application.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUser(UserCreateRequest request, CancellationToken cToken);
    Task<UserLoginRespose> Login(UserLoginRequest request, CancellationToken cToken);
    Task<UserLoginRespose> RefreshToken(Guid userId, string oldRefreshToken,CancellationToken cToken);
    Task<UserDto> GetByEmail(string email, CancellationToken cToken);
    Task<UserDto> GetById(Guid id, CancellationToken cToken);
    Task<List<UserAddressResponseDto>> GetAddressesByUserId(Guid id, CancellationToken cToken);
    Task<Guid> AddUserAddress(Guid userId, UserAddressCreateDto dto, CancellationToken cToken);
    Task ModifyUserAddress(Guid userId, Guid addressId, UserAddressModifyDto dto, CancellationToken cToken);
}