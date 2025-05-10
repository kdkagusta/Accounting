using Accounting.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Accounting.Server.Data
{
    public static class Seed
    {
        public static async Task SeedData(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new("Admin"),
                    new("User")
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com"
                };

                await userManager.CreateAsync(adminUser, "Pa$$w0rd");
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var normalUser = new ApplicationUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com"
                };

                await userManager.CreateAsync(normalUser, "Pa$$w0rd");
                await userManager.AddToRoleAsync(normalUser, "User");
            }

            if (!context.Products.Any())
            {
                var products = new List<Products>
                {
                    new() { Name = "Product 1", Price = 10.99m },
                    new() { Name = "Product 2", Price = 20.50m },
                    new() { Name = "Product 3", Price = 15.75m }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
