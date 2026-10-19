using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities.Products;

namespace DPBack.Infrastructure.Seeder;

public static class DeliveryOptionSeeder
{
    public static void SeedAsync(OrderStoreDbContext context)
    {
        if (context.DeliveryOptions.Any(x => x.Title == "Paczkomat InPost"))
        {
            context.DeliveryOptions.Add(new DeliveryTypeEntity
                { Id = Guid.NewGuid(), Title = "Paczkomat InPost", Price = 12.99m });
        }
    }
}