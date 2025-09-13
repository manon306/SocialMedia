namespace SocialMedia.BLL.Service.Implementation
{
    public class FollowSerives : IFollowSerives
    {
        private readonly SocialMediaDbContext db;
        private readonly IMapper mapper;

        public FollowSerives(SocialMediaDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task FollowUser(string followerId, string followingId)
        {
            var follow = new Follow { FollowerId = followerId, FollowingId = followingId };
            db.Follows.Add(follow);
            await db.SaveChangesAsync();
        }

        public async Task UnfollowUser(string followerId, string followingId)
        {
            var follow = await db.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow != null)
            {
                db.Follows.Remove(follow);
                await db.SaveChangesAsync();
            }
        }

    }
}