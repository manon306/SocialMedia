namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext :IdentityDbContext<User>
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Share>()
                .HasKey(ps => ps.ID);

            modelBuilder.Entity<Share>()
                .HasOne(ps => ps.Post)
                .WithMany(p => p.Shares)
                .HasForeignKey(ps => ps.PostId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Share>()
                .HasOne(ps => ps.User)
                .WithMany(u => u.Shares)
                .HasForeignKey(ps => ps.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
               .HasOne(c => c.Sender)
               .WithMany()
               .HasForeignKey(c => c.SenderId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.Receiver)
                .WithMany()
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Connection>()
                .HasIndex(c => new { c.SenderId, c.ReceiverId })
                .IsUnique();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
       public DbSet<Reply> Reply { get; set; }
        public DbSet<React> Reacts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }


}

