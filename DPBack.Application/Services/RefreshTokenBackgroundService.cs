using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Services;

public class RefreshTokenBackgroundService(IServiceScopeFactory scopeFactory, IOptions<RefreshTokenOptions> options)
    : BackgroundService
{
    private readonly RefreshTokenOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userRepo = scope.ServiceProvider.GetRequiredService<IUsersRepository>();
                var result = await userRepo.DeleteExpiredTokensAsync(stoppingToken);
            }
        
            await Task.Delay(TimeSpan.FromHours(_options.BackgroundServiceRunIntervalInHours), stoppingToken);
        }
    }
}