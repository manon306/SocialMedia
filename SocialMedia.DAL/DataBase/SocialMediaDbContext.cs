using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.DAL.Entity;
namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext :IdentityDbContext<User>
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }
        public virtual DbSet<User> user { get; set; }

    }
}
