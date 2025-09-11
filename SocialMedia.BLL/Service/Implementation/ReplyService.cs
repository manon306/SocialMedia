namespace SocialMedia.BLL.Service.Implementation
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepo repo;
        private readonly IMapper mapper;
        public ReplyService(IReplyRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public (bool, string) AddComment(AddReplyVm comment)
        {
            //validation
            if (comment == null)
            {
                return (false, "Cannot be null");
            }
            //Mapping
            var commentEntity = new Reply(comment.Content, comment.ParentCommentID, comment.CreatedBy);
            if (commentEntity == null)
            {
                return (false, "faild to create new Object");
            }
            //use repo
            var result = repo.AddReply(commentEntity);
            if (result.Item1 == false)
            {
                return (false, result.Item2);
            }
            return (true, null);
        }
        public (bool, string) DeleteComment(DeleteCommentVm comment)
        {
            //validation
            if (comment == null)
            {
                return (false, "Cannot delete empty Comment");
            }
            // use repo
            var result = repo.DeleteReply(comment.Id, comment.DeletedBy);
            if (result.Item1 == false)
            {
                return (false, result.Item2);
            }
            return (true, null);
        }
        public (bool, string) UpdateComment(UpdateCommentVm comment)
        {
            //validation
            if (comment == null)
            {
                return (false, "Cannot be null");
            }
            //repo
            var result = repo.UpdateReply(comment.ID, comment.Content, comment.UpdatedBy);
            if (result.Item1 == false)
            {
                return (false, result.Item2);
            }
            return (true, null);
        }
        public (bool, string, List<GetCommentVm>) GetAllComment(int postId)
        {
            //validation
            if (postId <= 0)
            {
                return (false, "No Post Found", null);
            }
            //repo
            var result = repo.GetAllReplies(postId);
            var entity = mapper.Map<List<GetCommentVm>>(result.Item3);
            if (result.Item1 == false)
            {
                return (false, result.Item2, null);
            }
            return (true, null, entity);
        }
    }
}
