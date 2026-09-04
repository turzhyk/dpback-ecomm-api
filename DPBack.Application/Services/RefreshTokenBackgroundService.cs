using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Services;

public class RefreshTokenBackgroundService:BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RefreshTokenOptions _options;

    public RefreshTokenBackgroundService(IServiceScopeFactory scopeFactory, IOptions<RefreshTokenOptions> options)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userRepo = scope.ServiceProvider.GetRequiredService<IUsersRepository>();
                var result = await userRepo.DeleteExpiredTokensAsync(stoppingToken);
            }
        
            await Task.Delay(TimeSpan.FromHours(_options.BackgroundServiceRunIntervalInHours), stoppingToken);
        }
    }
}