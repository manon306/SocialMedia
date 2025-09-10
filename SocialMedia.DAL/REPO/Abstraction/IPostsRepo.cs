namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IPostsRepo
    {
        (bool , string) AddPost(Post post);
        (bool, string) UpdatePost(Post post);
        (bool, string) DeletePost(int postId, string deletedBy);
        (bool , string ,List<Post>) GetPosts();
        (bool, string, Post) GetPostById(int postId);
        (bool, string, List<Share>) GetAll();
        (bool, string, List<Post>) GetArchivedPosts();
        (bool, string, List<Post>) GetSavedPosts();
        public (bool, string) ToggleSavePost(int postId);
        (bool, string) ToggleArchievePost(int postId);
        (bool, string, List<Post>) unArchive();
        (bool, string, Post) SharePost(int postId, string userId, string? content);
    }
}
