using System.Threading.Channels;
using DPBack.Application.Abstractions;
using DPBack.Application.Mappers;
using DPBack.Application.Mappers.Config;
using DPBack.Application.Options.Pricing;
using DPBack.Application.Pricing;
using DPBack.Application.Pricing.Calculators;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DPBack.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services
    )
    {
        services.AddScoped<IOrdersService, OrdersService>();
       
        services.AddScoped<IPriceCalcService, PriceCalcService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IReceiptService, OrderReceiptService>();

        services.AddMemoryCache();


        services.AddScoped<IPriceCalculator, BusinesscardCalculator>();
        services.AddScoped<IPriceCalculator, OpeningHoursStickerCalculator>();
        services.AddScoped<IPriceCalculator, TshirtCalculator>();

        var channel = Channel.CreateUnbounded<Guid>(new UnboundedChannelOptions
            { SingleReader = true});
        services.AddSingleton(channel);
        services.AddSingleton(channel.Writer);
        services.AddSingleton(channel.Reader);
    //Config
        services.AddScoped<IProductConfigMapper, BusinesscardsConfigMapper>();
        services.AddScoped<IProductConfigMapper, TshirtConfigMapper>();

        services.AddScoped<PriceCalculatorStrategy>();
        services.AddScoped<IProductConfigMapperResolver,ProductConfigMapperResolver>();
        return services;
    }
}