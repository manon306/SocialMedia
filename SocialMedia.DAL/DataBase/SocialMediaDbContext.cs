namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext :IdentityDbContext
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<React> Reacts { get; set; }

    }
}
