using System.Formats.Asn1;
using System.Threading.Channels;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DPBack.Application.Services;

public class OrderReceiptBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ChannelReader<Guid> channelReader,
    ILogger<OrderReceiptBackgroundService> logger,
    IOptions<SmtpOptions> smtpOptions)
    : BackgroundService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

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
        logger.LogInformation("Processing receipt for {Id}", id);
        using var scope = serviceScopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
        var receiptService = scope.ServiceProvider.GetRequiredService<IReceiptService>();
        logger.LogInformation("Generating PDF for {OrderId}", id);
        try
        {
            var task = await repo.GetOrderReceiptTaskAsync(id, cToken);
            if (task is null)
                throw new KeyNotFoundException("Receipt task not found");
            var orderId = task.OrderId;
            logger.LogInformation("order id is {Id}", orderId);
            var (resultPdf, fileName, targetEmail) = await receiptService.GetReceiptFileAsync(orderId, cToken);
            var sender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            logger.LogInformation("PDF generated ({Size} bytes), sending to {Email}", resultPdf.Length, targetEmail);
            await sender.SendReceiptEmailAsync(targetEmail, resultPdf, fileName, _smtpOptions, cToken);
            logger.LogInformation("Email sent for {OrderId}", id);
            await repo.ChangeOrderReceiptStatusAsync(id, OrderReceiptStatus.Done, cToken);
            logger.LogInformation($"Finished receipt for {id}");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while processing receipt {Id}", id);
            throw;
        }
    }

    private async Task ProcessFallbackReceiptTasksAsync(CancellationToken cToken)
    {
        logger.LogInformation("Starting processing fallback receipt tasks");
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (await timer.WaitForNextTickAsync(cToken))
        {
            using var scope = serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
            var stuckTaskIds = await repo.GetOrderReceiptTasksWithStatusAsync(OrderReceiptStatus.Pending, cToken);
            logger.LogInformation($"found {stuckTaskIds.Count} order receipt tasks");
            foreach (var id in stuckTaskIds)
            {
                await ProcessTaskAsync(id, cToken);
            }
        }
    }
}