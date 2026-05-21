
using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using DPBack.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserStoreDbContext _context;

    public UsersRepository(UserStoreDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserWithIdExists(Guid id, CancellationToken cToken)
    {
        return  await _context.Users.AnyAsync(x => x.Id == id, cToken);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cToken);
        if (userEntity == null)
            return null;
        return new User(userEntity.Id, userEntity.Login, userEntity.PasswordHash, userEntity.Email, userEntity.Role,
            userEntity.CreatedAt);
    }
    public async Task<User?> GetById(Guid id, CancellationToken cToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cToken);
        if (userEntity == null)
            return null;
        return new User(userEntity.Id, userEntity.Login, userEntity.PasswordHash, userEntity.Email, userEntity.Role,
            userEntity.CreatedAt);
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cToken)
    {
        var userEntity = new UserEntity(user.Id, user.Login, user.PasswordHash, user.Email, user.Role, user.CreatedAt);
        await _context.Users.AddAsync(userEntity, cToken);
        await _context.SaveChangesAsync(cToken);
        return user.Id;
    }

    public async Task<List<UserAddress>>  GetAdressesByUserId(Guid id, CancellationToken cToken)
    {
        var entities = await _context.Adresses
            .Where(address => address.UserId == id)
            .ToListAsync(cToken);
        if (entities.Count == 0)
            return new List<UserAddress>();

        return entities.Select(entity => entity.ToModel()).ToList();
    }

    public async Task AddUserAddress( UserAddress address, CancellationToken cToken)
    {
        var entity =  new UserAdressEntity(address.Id, address.UserId, address.Country, address.City, address.Street,
            address.BuildingNumber, address.ApartmentNumber, address.PostalCode, address.PhoneNumber, address.Email,
            address.Options);

        await _context.Adresses.AddAsync(entity, cToken);
        await _context.SaveChangesAsync(cToken);
    }

    public async Task<bool> AddressWithIdExists(Guid id, CancellationToken cToken)
    {
        return await _context.Adresses.AnyAsync(x => x.Id == id, cToken);
    }

    public async Task<UserAddress?> GetAddressById(Guid id, CancellationToken cToken)
    {
        var result = await _context.Adresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cToken);
        return result?.ToModel();
    }

    public async Task UpdateUserAddress(Guid addressId, UserAddress address, CancellationToken cToken)
    {
        var entity = await _context.Adresses
            .FirstOrDefaultAsync(x => x.Id == addressId, cToken);
        if (entity is null)
            throw new KeyNotFoundException("address not found");
        entity.Country = address.Country;
        entity.City = address.City;
        entity.Street = address.Street;
        entity.BuildingNumber = address.BuildingNumber;
        entity.ApartmentNumber = address.ApartmentNumber;
        entity.PostalCode = address.PostalCode;
        entity.PhoneNumber = address.PhoneNumber;
        entity.Email = address.Email;
        entity.Options = address.Options;
        await _context.SaveChangesAsync(cToken);
    }
}