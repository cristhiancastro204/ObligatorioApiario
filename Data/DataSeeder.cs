using Microsoft.AspNetCore.Identity;

namespace ObligatorioApiario.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Dueño", "Peon" };

            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync("admin@apiario.com");
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = "admin@apiario.com",
                    Email = "admin@apiario.com",
                    EmailConfirmed = true
                };

                var createPowerUser = await userManager.CreateAsync(newAdmin, "Apiario2026!");
                if (createPowerUser.Succeeded)
                {
                    // Assign to Dueño role
                    await userManager.AddToRoleAsync(newAdmin, "Dueño");
                }
            }
        }
    }
}
