using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Entities;

namespace OnlineStore.Infrastructure.Data.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<Client> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Admin User
        await SeedAdminUserAsync(userManager);

        // Seed Categories
        await SeedCategoriesAsync(context);

        // Seed Sample Products
        await SeedProductsAsync(context);

        // Seed Promo Codes
        await SeedPromoCodesAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<Client> userManager)
    {
        var adminEmail = "admin@onlinestore.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new Client
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var categories = new[]
        {
            new Category { Name = "Electronics", Description = "Electronic devices and accessories" },
            new Category { Name = "Clothing", Description = "Apparel and fashion items" },
            new Category { Name = "Books", Description = "Books and literature" },
            new Category { Name = "Home & Garden", Description = "Home improvement and garden supplies" },
            new Category { Name = "Digital Software", Description = "Digital products and software licenses" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var electronicsCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Electronics");
        var clothingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Clothing");
        var booksCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Books");
        var digitalCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Digital Software");

        if (electronicsCategory == null || clothingCategory == null || booksCategory == null || digitalCategory == null)
            return;

        var products = new[]
        {
            new Product
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with long battery life",
                PhotoPath = "https://res.cloudinary.com/dnjledoti/image/upload/v1766072580/mouse_thlg5q.png",
                Price = 29.99m,
                CategoryId = electronicsCategory.Id,
                StockQuantity = 50
            },
            new Product
            {
                Name = "Mechanical Keyboard",
                Description = "Premium mechanical keyboard with RGB backlighting",
                PhotoPath = "https://res.cloudinary.com/dnjledoti/image/upload/v1766072580/keyboard_elffrn.png",
                Price = 89.99m,
                CategoryId = electronicsCategory.Id,
                StockQuantity = 40
            },
            new Product
            {
                Name = "Cotton T-Shirt",
                Description = "100% cotton comfortable t-shirt",
                PhotoPath = "https://res.cloudinary.com/dnjledoti/image/upload/v1766072580/t-shirt_kcu4v3.png",
                Price = 19.99m,
                CategoryId = clothingCategory.Id,
                StockQuantity = 100
            },
            new Product
            {
                Name = "Programming Book",
                Description = "Comprehensive guide to software development",
                Price = 49.99m,
                CategoryId = booksCategory.Id,
                StockQuantity = 30
            },
            new Product
            {
                Name = "Premium Software License",
                Description = "One-year license for premium software",
                Price = 99.99m,
                CategoryId = digitalCategory.Id,
                StockQuantity = 999 // Digital products have high stock
            },
            new Product
            {
                Name = "USB-C Cable",
                Description = "High-speed USB-C charging cable",
                Price = 12.99m,
                CategoryId = electronicsCategory.Id,
                StockQuantity = 75
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPromoCodesAsync(ApplicationDbContext context)
    {
        if (await context.PromoCodes.AnyAsync())
            return;

        var promoCodes = new[]
        {
            new PromoCode
            {
                Code = "WELCOME10",
                DiscountPercentage = 10,
                ValidFrom = DateTime.UtcNow.AddDays(-30),
                ValidTo = DateTime.UtcNow.AddDays(30),
                IsActive = true
            },
            new PromoCode
            {
                Code = "SAVE20",
                DiscountPercentage = 20,
                ValidFrom = DateTime.UtcNow.AddDays(-15),
                ValidTo = DateTime.UtcNow.AddDays(15),
                IsActive = true
            },
            new PromoCode
            {
                Code = "EXPIRED",
                DiscountPercentage = 15,
                ValidFrom = DateTime.UtcNow.AddDays(-60),
                ValidTo = DateTime.UtcNow.AddDays(-30),
                IsActive = false
            }
        };

        await context.PromoCodes.AddRangeAsync(promoCodes);
        await context.SaveChangesAsync();
    }
}



