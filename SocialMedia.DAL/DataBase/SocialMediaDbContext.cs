using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext :IdentityDbContext<User>
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<React> reacts { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Posts> posts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

    }
}
