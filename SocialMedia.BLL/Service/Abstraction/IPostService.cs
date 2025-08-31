namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IPostService
    {
        (bool, string) AddPost(CreateVm post);
        (bool, string) DeletePost(int postId, string deletedBy);
        (bool, string) UpdatePost(updatePostVm post);
        (bool, string, List<PostVm>) GetPosts();
        (bool, string, List<PostVm>) GetSavedPosts();
        (bool, string, List<PostVm>) GetArchivedPosts();

    }
}
