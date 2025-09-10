namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IuserRepo
    {
        bool Create(User user);
        List<User> SearchUser(string keyword);
        List<User> GetUsers();
    }
}