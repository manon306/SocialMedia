using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.DAL.ENUM;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.DAL.DataBase
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Check if roles already exist and create them if they don't
            foreach (var roleName in Enum.GetNames(typeof(Role)))
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task<bool> AssignRoleToUserAsync(UserManager<Entity.User> userManager, string userId, Role role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roleName = Enum.GetName(typeof(Role), role);
            var result = await userManager.AddToRoleAsync(user, roleName);
            
            return result.Succeeded;
        }
    }
}