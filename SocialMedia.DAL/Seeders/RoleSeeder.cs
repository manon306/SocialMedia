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
        // ⬅️ Seed الأدوار لو مش موجودة
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in Enum.GetNames(typeof(Role)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        // ⬅️ إسناد الدور لمستخدم
        public static async Task<bool> AssignRoleToUserAsync(UserManager<Entity.User> userManager, string userId, Role role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var roleName = Enum.GetName(typeof(Role), role);

            // لو هو مش أصلاً في الدور، أضفه
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }

            return true;
        }
    }
}
