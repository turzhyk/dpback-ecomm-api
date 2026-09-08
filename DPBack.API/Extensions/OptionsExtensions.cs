using DPBack.Application.Options;
using DPBack.Application.Options.Pricing;

namespace DPBack.API.Extensions;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<PayUOptions>(configuration.GetSection("PayU"));
        services.Configure<Pricing>(configuration.GetSection("Pricing"));
        services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshToken"));
        services.Configure<CompanyOptions>(configuration.GetSection("Company"));
        services.Configure<SmtpOptions>(configuration.GetSection("SMTP"));
        return services;
    }
}