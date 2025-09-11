namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IReactRepo
    {
        (bool, string) AddReact(React react);
        (bool, string) UpdateReact(React react);
        (bool, string) DeleteReact(int reactId, string deletedBy);
        (bool, string, React) GetReactById(int reactId);
        (bool, string, List<React>) GetReactsByPostId(int postId);
        (bool, string, React) GetUserReactForPost(int postId, string userId);
    }
}
