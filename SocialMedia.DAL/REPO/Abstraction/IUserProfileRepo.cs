

namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IUserProfileRepo
    {
        //void Create(UserProfile profile);
        //UserProfile GetByUserId(string userId);

        Task Create(UserProfile profile);
        Task<UserProfile> GetByUserId(string userId);

        //bool Create(User user);
        Task Update(UserProfile profile);
        Task Delete(UserProfile profile);
        List<User> GetUsers(Expression<Func<User, bool>>? filter = null);

    }
}
