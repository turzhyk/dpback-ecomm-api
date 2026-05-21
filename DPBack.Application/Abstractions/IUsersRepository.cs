

using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IUsersRepository
{
    Task<Guid> CreateAsync(User user, CancellationToken cToken);
    Task<User?> GetByEmail(string email, CancellationToken cToken);
    Task<bool> UserWithIdExists(Guid id, CancellationToken cToken);
    Task<User?> GetById(Guid id, CancellationToken cToken);
    Task<List<UserAddress>> GetAdressesByUserId(Guid id, CancellationToken cToken);
    Task AddUserAddress( UserAddress address, CancellationToken cToken);
    Task<bool> AddressWithIdExists(Guid id, CancellationToken cToken);
    Task<UserAddress?> GetAddressById(Guid id, CancellationToken cToken);
    Task UpdateUserAddress(Guid addressId, UserAddress dto, CancellationToken cToken);
}