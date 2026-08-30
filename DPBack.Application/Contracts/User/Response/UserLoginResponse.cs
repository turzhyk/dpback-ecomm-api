namespace DPBack.Application.Contracts;

public record UserLoginResponse(string Token, string RefreshToken);