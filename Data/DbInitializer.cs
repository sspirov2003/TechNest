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

        // Seed Products
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

        // Seed Orders
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
    }
}
