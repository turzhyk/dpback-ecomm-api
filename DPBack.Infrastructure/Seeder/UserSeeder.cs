using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DPBack.Infrastructure.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserStoreDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        if (!context.Users.Any(x => x.Login == "admin"))
        {
            var admin = new User(new Guid("71919e8c-047b-4404-808f-093a0dd00e71"), "admin", "", "admin@local", UserRole.Admin, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(admin, configuration["Accounts:BaseAdminPassword"]??"xxxx");
            var entity = new UserEntity(admin.Id, admin.Login, password, admin.Email, admin.Role, admin.CreatedAt);
            context.Users.Add(entity);
        }

        if (!context.Users.Any(x => x.Login == "worker1"))
        {
            var worker = new User(new Guid("29f84356-fde9-4cdd-8909-9ac8291bc9b1"), "worker1", "", "worker1@local", UserRole.Manager, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(worker, configuration["Accounts:BaseWorkerPassword"]??"xxxx");
            var entity = new UserEntity(worker.Id, worker.Login, password, worker.Email, worker.Role, worker.CreatedAt);
            context.Users.Add(entity);
        }
        if (!context.Users.Any(x => x.Login == "customer1"))
        {
            var customer = new User(new Guid("f33f1535-0f75-4888-8e2a-c9aa8b1599a4"), "customer1", "", "customer1@local", UserRole.User, DateTime.UtcNow);
            var password = passwordHasher.HashPassword(customer, configuration["Accounts:BaseCustomerPassword"]??"xxxx");
            var entity = new UserEntity(customer.Id, customer.Login, password, customer.Email, customer.Role, customer.CreatedAt);
            context.Users.Add(entity);
        }

        await context.SaveChangesAsync();
    }
}