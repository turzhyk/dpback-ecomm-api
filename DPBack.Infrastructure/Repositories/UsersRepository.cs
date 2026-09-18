using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using DPBack.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories;

public class UsersRepository(UserStoreDbContext context) : IUsersRepository
{
    public async Task<bool> UserWithIdExistsAsync(Guid id, CancellationToken cToken)
    {
        return await context.Users.AnyAsync(x => x.Id == id, cToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cToken)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cToken);
        if (userEntity == null)
            return null;
        return new User
        {
            Id = userEntity.Id,
            Login = userEntity.Login,
            PasswordHash = userEntity.PasswordHash,
            Email = userEntity.Email,
            Role = userEntity.Role,
            CreatedAt = userEntity.CreatedAt
        };
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cToken)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.Id == id, cToken);
        if (userEntity == null)
            return null;
        return new User
        {
            Id = userEntity.Id,
            Login = userEntity.Login,
            PasswordHash = userEntity.PasswordHash,
            Email = userEntity.Email,
            Role = userEntity.Role,
            CreatedAt = userEntity.CreatedAt
        };
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cToken)
    {
        var userEntity = new UserEntity(user.Id, user.Login, user.PasswordHash, user.Email, user.Role, user.CreatedAt);
        await context.Users.AddAsync(userEntity, cToken);
        await context.SaveChangesAsync(cToken);
        return user.Id;
    }

 

    public async Task AddRefreshTokenAsync(User user, string token, CancellationToken cToken)
    {
        var tokenEntity = new RefreshTokenEntity
            { Id = Guid.NewGuid(), UserId = user.Id, Token = token, ExpiresAt = DateTime.UtcNow.AddDays(30) };
        await context.RefreshTokens.AddAsync(tokenEntity, cToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cToken)
    {
        var entity = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cToken);
        if (entity == null)
            return null;
        return new RefreshToken
            { Id = entity.Id,UserId = entity.UserId, Token = entity.Token, ExpiresAt = entity.ExpiresAt, IsRevoked = entity.IsRevoked };
    }

    public async Task<(RefreshToken?, User?)> GetRefreshTokenWithUserByTokenAsync(string token,
        CancellationToken cToken)
    {
        var tokenEntity = await context.RefreshTokens.Include(u => u.User)
            .FirstOrDefaultAsync(x => x.Token == token, cToken);
        if (tokenEntity == null)
            return (null, null);
        var userEntity = tokenEntity.User;
        return (new RefreshToken
        {
            Id = tokenEntity.Id, Token = tokenEntity.Token, ExpiresAt = tokenEntity.ExpiresAt,
            IsRevoked = tokenEntity.IsRevoked
        }, userEntity.ToModel());
    }

    public async Task SetTokenRevokedAsync(string token, CancellationToken cToken)
    {
        var entity = await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cToken);
        if (entity != null)
            entity.IsRevoked = true;
        await context.SaveChangesAsync(cToken);
    }

    public async Task<int> DeleteExpiredTokensAsync(CancellationToken cToken)
    {
        var now = DateTime.UtcNow;
        return await context.RefreshTokens.Where(x => x.ExpiresAt < now).ExecuteDeleteAsync(cToken);
    }

    public async Task SaveChangesAsync(CancellationToken cToken)
    {
        await context.SaveChangesAsync(cToken);
    }
}