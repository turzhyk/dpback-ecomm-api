using DPBack.Application.Services;

namespace DPBack.API.Extensions;

public static class BackgroundServicesExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<RefreshTokenBackgroundService>();
        return services;
    }
}