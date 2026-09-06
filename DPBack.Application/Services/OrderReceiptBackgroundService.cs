using System.Formats.Asn1;
using System.Threading.Channels;
using DPBack.Application.Abstractions;
using DPBack.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DPBack.Application.Services;

public class OrderReceiptBackgroundService(IServiceScopeFactory serviceScopeFactory, ChannelReader<Guid> channelReader)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channelTask = ListenChannelAsync(stoppingToken);
        var fallbackTasks = ProcessFallbackReceiptTasksAsync(stoppingToken);
        await Task.WhenAll(channelTask, fallbackTasks);
    }

    private async Task ListenChannelAsync(CancellationToken cToken)
    {
        await foreach (var taskId in channelReader.ReadAllAsync(cToken))
        {
            await ProcessTaskAsync(taskId, cToken);
        }

        ;
    }

    private async Task ProcessTaskAsync(Guid id, CancellationToken cToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
        
        
        await repo.ChangeOrderReceiptStatusAsync(id, OrderReceiptStatus.Done, cToken);
    }

    private async Task ProcessFallbackReceiptTasksAsync(CancellationToken cToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
        while (await timer.WaitForNextTickAsync(cToken))
        {
            using var scope= serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
            var stuckTaskIds = await repo.GetOrderReceiptTasksWithStatusAsync(OrderReceiptStatus.Pending, cToken);
            foreach (var id in stuckTaskIds)
            {
                await ProcessTaskAsync(id, cToken);
            }
        }
    }
}