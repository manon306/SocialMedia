using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SocialMedia.DAL.Entity;

namespace SocialMedia.DAL.DataBase
{
    public class SocialMediaDbContext : IdentityDbContext<User>
    {
        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<React> Reacts { get; set; }
        public DbSet<Job> Jobs { get; set; }

        // 🔹 New Messaging system
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Conversation
            builder.Entity<Conversation>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.IsGroup).IsRequired();
                b.Property(x => x.GroupName).HasMaxLength(200);
            });

            // ConversationParticipant
            builder.Entity<ConversationParticipant>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.ConversationId, x.UserId }).IsUnique();

                b.HasOne(x => x.Conversation)
                 .WithMany(c => c.Participants)
                 .HasForeignKey(x => x.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Message
            builder.Entity<Message>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.ConversationId, x.CreatedAt });
                b.Property(x => x.Body).IsRequired().HasMaxLength(4000);

                b.HasOne(x => x.Conversation)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(x => x.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}