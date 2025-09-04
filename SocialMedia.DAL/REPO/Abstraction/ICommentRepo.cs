namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface ICommentRepo
    {
        (bool, string) AddComment(Comment comment);
        (bool, string) UpdateComment(Comment comment);
        (bool, string) DeleteComment(int commentId, string Deletedby);
        (bool, string, List<Comment>) GetAllComments(int PostId);
    }
}
