namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IPostService
    {
        (bool, string) AddPost(CreateVm post, string userId);
        (bool, string) DeletePost(int postId, string deletedBy);
        (bool, string) UpdatePost(updatePostVm post);
        (bool, string, Post) GetById(int id);
        (bool, string, List<PostVm>) GetPosts();
        (bool, string, List<PostVm>) GetSavedPosts();
        (bool, string, List<PostVm>) GetArchivedPosts();
        (bool, string) toggleSaved(int PostId);
        (bool, string) toggleArchive(int PostId);
        void UnArchiveAllPosts();
    }
}

