using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DPBack.Infrastructure;

public class DatabaseInitializer(
    OrderStoreDbContext orderStoreDbContext,
    UserStoreDbContext userStoreDbContext,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration, ILogger<DatabaseInitializer> logger
    ) : IDatabaseInitializer
{
    public async Task InitializeDatabaseAsync()
    {
        logger.LogInformation("Initializing database ...");
        try
        {
            Console.WriteLine("START MIGRATION");
            await orderStoreDbContext.Database.MigrateAsync();
            await userStoreDbContext.Database.MigrateAsync();
            await UserSeeder.SeedAsync(userStoreDbContext, passwordHasher, configuration);
            await CustomerSeeder.SeedAsync(orderStoreDbContext);
            Console.WriteLine("MIGRATION DONE");
            logger.LogInformation("Done initializing database ...");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            Console.WriteLine(e);
            throw;
        }
    }
}