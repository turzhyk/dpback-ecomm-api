using DPBack.Application.Validators.Customers;
using DPBack.Application.Validators.Orders;
using FluentValidation;

namespace DPBack.API.Extensions;

public static class ValidatorsExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        
        services.AddValidatorsFromAssemblyContaining<CustomerCreateRequestValidator>();
        return services;
    }
}