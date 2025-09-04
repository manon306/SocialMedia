namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface ICommentRepo
    {
        (bool, string) AddComment(Comment comment);
        (bool, string) UpdateComment(Comment comment);
        (bool, string) DeletePost(int commentId, string Deletedby);
    }
}
