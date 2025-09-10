

using Microsoft.EntityFrameworkCore;

namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly SocialMediaDbContext db;

        public UserProfileRepo(SocialMediaDbContext db)
        {
            this.db = db;
        }

        public List<User> GetUsers(Expression<Func<User, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    var user = db.Users.Where(filter).ToList();
                    return user;

                }
                else
                {
                    var user = db.Users.ToList();
                    return user;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task Create(UserProfile profile)
        {
            await db.UserProfiles.AddAsync(profile);
            await db.SaveChangesAsync();
        }


        public async Task<UserProfile?> GetByUserId(string userId)
        {
            return await db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }


        public async Task Update(UserProfile profile)
        {
            db.UserProfiles.Update(profile);
            await db.SaveChangesAsync();
        }

        public async Task Delete(UserProfile profile)
        {
            db.UserProfiles.Remove(profile);
            await db.SaveChangesAsync();
        }

    }
}