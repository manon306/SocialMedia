using SocialMedia.DAL.Entity;

namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IPostsRepo
    {
        (bool , string) AddPost(Entity.Post post);
        (bool, string) UpdatePost(Entity.Post post);
        (bool, string) DeletePost(int postId, string deletedBy);
        (bool , string ,List<Post>) GetPosts();
        (bool, string, Post) GetPostById(int postId);
        (bool, string, List<Post>) GetArchivedPosts();
        (bool, string, List<Post>) GetSavedPosts();
    }
}
