using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Data
{
    public class DataSeeding
    {
        public static async Task RolesSeeding(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if(!await roleManager.RoleExistsAsync(RolesType.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(RolesType.Role_Admin));
            }

            if(!await roleManager.RoleExistsAsync(RolesType.Role_Employee))
            {
                await roleManager.CreateAsync(new IdentityRole(RolesType.Role_Employee));
            }
        }

        public static async Task AdminSeeding(IServiceProvider serviceProvider)
        {

            var configurationService = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var email = configurationService["AdminUser:Email"];
            var password = configurationService["AdminUser:Password"];

            if(email != null && password != null)
            {   
                if(await userManager.FindByEmailAsync(email) is null) 
                {
                    var user = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = string.Empty,
					    Employee = new Employee { Name = "Admin", IsAdmin = true},              
                    };

                    var result = userManager.CreateAsync(user, password).Result;

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RolesType.Role_Admin);           
                    }
                }
            }
        }
    }
}
