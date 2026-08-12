using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Seeder;

public class CustomerSeeder
{
    public static async Task SeedAsync(OrderStoreDbContext context)
    {
        if (!await context.Customers.AnyAsync(x => x.Phone == "066123456"))
            context.Customers.Add(new CustomerEntity
            {
                Id = new Guid("1e7f7772-9c53-4e88-afa0-d785b3db9842"), Name = "Weronika Nowak", Phone = "066123456",
                Email = "w.nowak@gmail.com"
            });
        if (!await context.Customers.AnyAsync(x => x.Phone == "066222333"))
            context.Customers.Add(new CustomerEntity
            {
                Id = new Guid("783918b5-d554-4a2e-8fa2-35885848ec47"), Name = "paweł wojnielowicz", Phone = "066222333",
                Email = "woj1@gmail.com"
            });
        await context.SaveChangesAsync();
    }
}