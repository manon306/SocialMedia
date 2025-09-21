
//بحط هنا الي عايزة استخدمه من ال          user in Identity
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

            // ✅ الصورة (ImagePath) — هنضيفها كـ Claim مخصص
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                identity.AddClaim(new Claim("ImagePath", user.ImagePath));
            }
            return identity;
        }
    }
}