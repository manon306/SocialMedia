

namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly SocialMediaDbContext _db;

        public UserProfileRepo(SocialMediaDbContext db)
        {
            _db = db;
        }

        public async Task Create(UserProfile profile)
        {
            await _db.UserProfiles.AddAsync(profile);
            await _db.SaveChangesAsync();
        }

        public async Task<UserProfile?> GetByUserId(string userId)
        {
            return await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
