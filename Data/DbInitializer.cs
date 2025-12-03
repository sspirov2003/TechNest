using TechNestClean.Models;

namespace TechNestClean.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Create database if not exists
        context.Database.EnsureCreated();

        // Seed Users
        if (!context.Users.Any())
        {
            var users = new[]
            {
                new AppUser { Name = "Alice", Role = UserRole.Admin },
                new AppUser { Name = "Bob", Role = UserRole.Moderator },
                new AppUser { Name = "Charlie", Role = UserRole.User }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
        // Seed Categories
        if (!context.Categories.Any())
        {
            var categories = new[]
            {
                new Category { Name = "Laptops" },
                new Category { Name = "Accessories" },
                new Category { Name = "Smartphones" },
                new Category { Name = "Peripherals" }
         };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }


        if (!context.Products.Any())
        {
            var products = new[]
            {
                new Product { Name = "Gaming Laptop", Price = 1599.99M },
                new Product { Name = "Wireless Mouse", Price = 49.99M },
                new Product { Name = "Mechanical Keyboard", Price = 119.99M },
                new Product { Name = "Headphones", Price = 89.99M },
                new Product { Name = "4K Monitor", Price = 399.99M }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }


        if (!context.Orders.Any())
        {
            var firstUser = context.Users.First(u => u.Name == "Charlie");
            var firstProduct = context.Products.First();

            var orders = new[]
            {
                new Order { AppUserId = firstUser.Id, ProductId = firstProduct.Id, OrderDate = DateTime.UtcNow }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }


        if (!context.PromoCodes.Any())
        {
            var promoCodes = new[]
            {
                new PromoCode
                {
                    Code = "SAVE10",
                    DiscountPercent = 10,
                    ExpiryDate = DateTime.Now.AddMonths(1),
                    IsActive = true
                },
                new PromoCode
                {
                    Code = "STUDENT15",
                    DiscountPercent = 15,
                    ExpiryDate = DateTime.Now.AddMonths(2),
                    IsActive = true
                },
                new PromoCode
                {
                    Code = "WELCOME5",
                    DiscountPercent = 5,
                    ExpiryDate = DateTime.Now.AddYears(1),
                    IsActive = true
                }
            };

            context.PromoCodes.AddRange(promoCodes);
            context.SaveChanges();
        }
        // Seed Order Statuses
        if (!context.OrderStatuses.Any())
        {
            var statuses = new[]
            {
                new OrderStatus { Name = "Processing" },
                new OrderStatus { Name = "Shipped out" },
                new OrderStatus { Name = "Delivered" }
            };
            context.OrderStatuses.AddRange(statuses);
            context.SaveChanges();
        }
        // Seed Order Statuses
        if (!context.OrderStatuses.Any())
        {
            var statuses = new[]
            {
                new OrderStatus { Name = "Processing" },
                new OrderStatus { Name = "Shipped out" },
                new OrderStatus { Name = "Delivered" }
            };
            context.OrderStatuses.AddRange(statuses);
            context.SaveChanges();
        }

    }
}
