using DPBack.Application.Abstractions;
using DPBack.Infrastructure.Payments;
using DPBack.Infrastructure.PayU;
using DPBack.Infrastructure.QuestPdfGenerator;
using DPBack.Infrastructure.Repositories;
using DPBack.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace DPBack.Infrastructure;

public static class InfrastructureDependency
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IReceiptGenerator, QuestPdfReceiptGenerator>();
        services.AddScoped<IEmailSender, MailKitEmailSender>();
        services.AddSingleton<IPaymentTokenProvider, PayUTokenProvider>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        services.AddHttpClient<IPaymentService, PayUService>(client =>
        {
            client.BaseAddress = new Uri(configuration["PayU:BaseAddress"]!);
        });
        
        QuestPDF.Settings.License = LicenseType.Community;
        return services;
    }
}