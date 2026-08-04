

using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IUsersRepository
{
    Task<Guid> CreateAsync(User user, CancellationToken cToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cToken);
    Task<bool> UserWithIdExistsAsync(Guid id, CancellationToken cToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cToken);
    Task<List<UserAddress>> GetAddressesByUserIdAsync(Guid id, CancellationToken cToken);
    Task AddUserAddressAsync( UserAddress address, CancellationToken cToken);
    Task<bool> AddressWithIdExists(Guid id, CancellationToken cToken);
    Task<UserAddress?> GetAddressByIdAsync(Guid id, CancellationToken cToken);
    Task UpdateUserAddressAsync(Guid addressId, UserAddress dto, CancellationToken cToken);
    Task AddRefreshTokenAsync(User user, string token, CancellationToken cToken);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cToken);
    Task SetTokenRevokedAsync(string token, CancellationToken cToken);
    
    
    Task SaveChangesAsync(CancellationToken cToken);
}