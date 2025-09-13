namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IFollowSerives
    {
        Task FollowUser(string followerId, string followingId);
        Task UnfollowUser(string followerId, string followingId);
    }
}
