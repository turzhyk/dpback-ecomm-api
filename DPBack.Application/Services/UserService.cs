using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Contracts.User.Response;
using DPBack.Application.Exceptions;
using DPBack.Application.Mappers.User;
using DPBack.Application.Validators;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPBack.Application.Services;

public class UserService(
    IUsersRepository repo,
    IPasswordHasher<User> passwordHasher,
    ITokenProvider tokenProvider,
    ILogger<UserService> logger)
    : IUserService
{
    public async Task<Guid> CreateUserAsync(UserCreateRequest request, CancellationToken cToken)
    {
        logger.LogInformation("Creating new user");
        if (!EmailValidator.IsValid(request.Email))
            throw new ArgumentException("Invalid email");

        if (await repo.GetByEmailAsync(request.Email, cToken) != null)
            throw new UserAlreadyExistsException(request.Email);

        var user = new User(
            Guid.NewGuid(),
            request.Email,
            "",
            request.Email,
            UserRole.User,
            DateTime.UtcNow
        );

        var hash = passwordHasher.HashPassword(user, request.Password);

        user.SetPassword(hash);

        await repo.CreateAsync(user, cToken);

        return user.Id;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken cToken)
    {
        logger.LogInformation($"Login user {request.Login}");
        var user = await repo.GetByEmailAsync(request.Login, cToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid login or password");

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result != PasswordVerificationResult.Success)
            throw new UnauthorizedAccessException("Invalid login or password");

        var token = tokenProvider.Create(user);
        var refreshToken = tokenProvider.CreateRefreshToken();
        await repo.AddRefreshTokenAsync(user, refreshToken, cToken);
        await repo.SaveChangesAsync(cToken);
        return new UserLoginResponse(token, refreshToken);
    }

    public async Task<UserLoginResponse> RefreshToken(string oldRefreshToken, CancellationToken cToken)
    {
        var refresh = await repo.GetRefreshTokenByTokenAsync(oldRefreshToken, cToken);
        if (refresh == null)
        {
            throw new UnauthorizedAccessException("Refresh token not found");
        }

        if (refresh.IsRevoked)
        {
            // throw new UnauthorizedAccessException("Refresh token is revoked");
        }

        if (refresh.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException(
                $"Refresh token expired. ExpiresAt: {refresh.ExpiresAt}, Now: {DateTime.UtcNow}");
        }

        logger.LogInformation($"Refresh user {refresh.UserId}");

        var user = await repo.GetByIdAsync(refresh.UserId, cToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid refresh token");


        var newToken = tokenProvider.Create(user);
        var newRefreshToken = tokenProvider.CreateRefreshToken();


        await repo.SetTokenRevokedAsync(oldRefreshToken, cToken);
        await repo.AddRefreshTokenAsync(user, newRefreshToken, cToken);
        await repo.SaveChangesAsync(cToken);

        var response = new UserLoginResponse(newToken, newRefreshToken);

        return response;
    }

    public async Task<UserResponse> GetByEmailAsync(string email, CancellationToken cToken)
    {
        // FLUENT VALIDATION email 
        var user = await repo.GetByEmailAsync(email, cToken);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var response = user.ToDto();
        return response;
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cToken)
    {
        var user = await repo.GetByIdAsync(id, cToken);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var response = user.ToDto();
        return response;
    }

    public async Task<List<UserAddressResponseDto>> GetAddressesByUserIdAsync(Guid id, CancellationToken cToken)
    {
        var exists = await repo.UserWithIdExistsAsync(id, cToken);
        if (!exists)
            throw new KeyNotFoundException("user not found");

        var addresses = await repo.GetAddressesByUserIdAsync(id, cToken);

        return addresses.Select(a => a.ToDto()).ToList();
    }

    public async Task<Guid> AddUserAddressAsync(Guid userId, UserAddressCreateDto dto, CancellationToken cToken)
    {
        var userExists = await repo.UserWithIdExistsAsync(userId, cToken);
        if (!userExists)
            throw new KeyNotFoundException("user not found");
        var guid = Guid.NewGuid();
        var userAddress = new UserAddress(guid, userId, dto.Country, dto.City, dto.Street,
            dto.BuildingNumber, dto.ApartmentNumber, dto.PostalCode, dto.PhoneNumber, dto.Email,
            dto.Options);
        await repo.AddUserAddressAsync(userAddress, cToken);
        return guid;
    }

    public async Task ModifyUserAddressAsync(Guid userId, Guid addressId, UserAddressModifyDto dto, CancellationToken cToken)
    {
        var userExists = await repo.UserWithIdExistsAsync(userId, cToken);
        var address = await repo.GetAddressByIdAsync(addressId, cToken);
        if (!userExists || address is null)
            throw new KeyNotFoundException("user or/and address not found");


        address.Country = dto.Country ?? address.Country;
        address.City = dto.City ?? address.City;
        address.Street = dto.Street ?? address.Street;
        address.BuildingNumber = dto.BuildingNumber ?? address.BuildingNumber;
        address.ApartmentNumber = dto.ApartmentNumber ?? address.ApartmentNumber;
        address.PostalCode = dto.PostalCode ?? address.PostalCode;
        address.PhoneNumber = dto.PhoneNumber ?? address.PhoneNumber;
        address.Email = dto.Email ?? address.Email;
        address.Options = dto.Options ?? address.Options;

        await repo.UpdateUserAddressAsync(addressId, address, cToken);
    }
}