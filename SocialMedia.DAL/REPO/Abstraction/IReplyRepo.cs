namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IReplyRepo
    {
        (bool, string) AddReply(Reply comment);
        (bool, string) UpdateReply(int commentId, string Content, string Updatedby);
        (bool, string) DeleteReply(int commentId, string Deletedby);
        (bool, string, List<Reply>) GetAllReplies(int PostId);
    }
}
