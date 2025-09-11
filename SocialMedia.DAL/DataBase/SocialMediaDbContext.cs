
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext :IdentityDbContext<User>
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }

        //public DbSet<Comment> Comments { get; set; }
        //public DbSet<Post> Posts { get; set; }
        //public DbSet<React> reacts { get; set; }
        public DbSet<User> user { get; set; }
        //public DbSet<Posts> posts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public DbSet<Follow> Follows { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Connection>()
                .HasOne(c => c.Sender)
                .WithMany()
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Connection>()
                .HasOne(c => c.Receiver)
                .WithMany()
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Connection>()
                .HasIndex(c => new { c.SenderId, c.ReceiverId })
                .IsUnique();


            //follow

            builder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany()
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany()
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict); // 
        }
      

    }
}
