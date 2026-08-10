using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Contracts.User.Response;
using DPBack.Application.Exceptions;
using DPBack.Application.Validators;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPBack.Application.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _repo;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<UserService> _logger;

    public UserService(IUsersRepository repo, IPasswordHasher<User> passwordHasher, ITokenProvider tokenProvider,
        ILogger<UserService> logger)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    public async Task<Guid> CreateUser(UserCreateRequest request, CancellationToken cToken)
    {
        _logger.LogInformation("Creating new user");
        if (!EmailValidator.IsValid(request.Email))
            throw new ArgumentException("Invalid email");

        if (await _repo.GetByEmailAsync(request.Email, cToken) != null)
            throw new UserAlreadyExistsException(request.Email);

        var user = new User(
            Guid.NewGuid(),
            request.Email,
            "",
            request.Email,
            UserRole.User,
            DateTime.UtcNow
        );

        var hash = _passwordHasher.HashPassword(user, request.Password);

        user.SetPassword(hash);

        await _repo.CreateAsync(user, cToken);

        return user.Id;
    }

    public async Task<UserLoginRespose> Login(UserLoginRequest request, CancellationToken cToken)
    {
        _logger.LogInformation($"Login user {request.Login}");
        var user = await _repo.GetByEmailAsync(request.Login, cToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid login or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result != PasswordVerificationResult.Success)
            throw new UnauthorizedAccessException("Invalid login or password");

        var token = _tokenProvider.Create(user);
        var refreshToken = _tokenProvider.CreateRefreshToken();
        await _repo.AddRefreshTokenAsync(user, refreshToken, cToken);
        await _repo.SaveChangesAsync(cToken);
        return new UserLoginRespose(token, refreshToken);
    }

    public async Task<UserLoginRespose> RefreshToken(string oldRefreshToken, CancellationToken cToken)
    {
        var refresh = await _repo.GetRefreshTokenByTokenAsync(oldRefreshToken, cToken);
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

        _logger.LogInformation($"Refresh user {refresh.UserId}");
        
        var user = await _repo.GetByIdAsync(refresh.UserId, cToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid refresh token");


        var newToken = _tokenProvider.Create(user);
        var newRefreshToken = _tokenProvider.CreateRefreshToken();


        await _repo.SetTokenRevokedAsync(oldRefreshToken, cToken);
        await _repo.AddRefreshTokenAsync(user, newRefreshToken, cToken);
        await _repo.SaveChangesAsync(cToken);

        var response = new UserLoginRespose(newToken, newRefreshToken);

        return response;
    }

    public async Task<UserDto> GetByEmail(string email, CancellationToken cToken)
    {
        // FLUENTVALIDATION email validaion
        var user = await _repo.GetByEmailAsync(email, cToken);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var response = new UserDto
        {
            Id = user.Id, Login = user.Login, Email = user.Email, CreatedAt = user.CreatedAt,
            Addresses = user.Adresses?.Select(a => new UserAddressResponseDto
            (
                a.Id,
                a.Country,
                a.City,
                a.Street,
                a.BuildingNumber,
                a.ApartmentNumber,
                a.PostalCode,
                a.PhoneNumber,
                a.Email,
                a.Options
            )).ToList() ?? new List<UserAddressResponseDto>(),
            Role = user.Role
        };
        return response;
    }

    public async Task<UserDto> GetById(Guid id, CancellationToken cToken)
    {
        var user = await _repo.GetByIdAsync(id, cToken);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var response = new UserDto
        {
            Id = user.Id, Login = user.Login, Email = user.Email, CreatedAt = user.CreatedAt,
            Addresses = user.Adresses?.Select(a => new UserAddressResponseDto
            (
                a.Id,
                a.Country,
                a.City,
                a.Street,
                a.BuildingNumber,
                a.ApartmentNumber,
                a.PostalCode,
                a.PhoneNumber,
                a.Email,
                a.Options
            )).ToList() ?? new List<UserAddressResponseDto>(),
            Role = user.Role
        };
        return response;
    }

    public async Task<List<UserAddressResponseDto>> GetAddressesByUserId(Guid id, CancellationToken cToken)
    {
        var exists = await _repo.UserWithIdExistsAsync(id, cToken);
        if (!exists)
            throw new KeyNotFoundException("user not found");

        var entities = await _repo.GetAddressesByUserIdAsync(id, cToken);

        return entities.Select(a => new UserAddressResponseDto
        (
            a.Id,
            a.Country,
            a.City,
            a.Street,
            a.BuildingNumber,
            a.ApartmentNumber,
            a.PostalCode,
            a.PhoneNumber,
            a.Email,
            a.Options
        )).ToList();
    }

    public async Task<Guid> AddUserAddress(Guid userId, UserAddressCreateDto dto, CancellationToken cToken)
    {
        var exists = await _repo.UserWithIdExistsAsync(userId, cToken);
        if (!exists)
            throw new KeyNotFoundException("user not found");
        var guid = Guid.NewGuid();
        var userAdress = new UserAddress(guid, userId, dto.Country, dto.City, dto.Street,
            dto.BuildingNumber, dto.ApartmentNumber, dto.PostalCode, dto.PhoneNumber, dto.Email,
            dto.Options);
        await _repo.AddUserAddressAsync(userAdress, cToken);
        return guid;
    }

    public async Task ModifyUserAddress(Guid userId, Guid addressId, UserAddressModifyDto dto, CancellationToken cToken)
    {
        var userExists = await _repo.UserWithIdExistsAsync(userId, cToken);
        var address = await _repo.GetAddressByIdAsync(addressId, cToken);
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

        await _repo.UpdateUserAddressAsync(addressId, address, cToken);
    }
}