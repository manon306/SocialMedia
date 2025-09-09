using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SocialMedia.PL.Factories
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // ضيف الـ Name اللي في الجدول (لو موجود) غير الـ UserName
            if (!string.IsNullOrEmpty(user.Name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            }
            else
            {
                // fallback لو Name فاضي → يعرض الـ UserName
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? user.Email));
            }

            return identity;
        }
    }
}