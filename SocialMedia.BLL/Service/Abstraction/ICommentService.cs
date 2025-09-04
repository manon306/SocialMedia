using SocialMedia.BLL.ModelVM.Comment;

namespace SocialMedia.BLL.Service.Abstraction
{
    public interface ICommentService
    {
        (bool, string) AddComment(AddCommentVm comment);
        (bool, string) DeleteComment(DeleteCommentVm comment);
        (bool, string) UpdateComment(UpdateCommentVm comment);
        (bool, string, List<GetCommentVm>) GetAllComment(int postId);
    }
}
