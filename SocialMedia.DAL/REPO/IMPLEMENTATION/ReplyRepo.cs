namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class ReplyRepo : IReplyRepo
    {
        private readonly SocialMediaDbContext DB;
        public ReplyRepo(SocialMediaDbContext DB)
        {
            this.DB = DB;
        }

        // إضافة رد
        public (bool, string) AddReply(Reply reply)
        {
            if (reply == null)
            {
                return (false, "Reply is required");
            }

            DB.Reply.Add(reply);
            DB.SaveChanges();
            return (true, null);
        }

        // تحديث رد
        public (bool, string) UpdateReply(int replyId, string content, string updatedBy)
        {
            if (string.IsNullOrEmpty(content))
                return (false, "Reply content cannot be null");

            var existing = DB.Reply.FirstOrDefault(r => r.Id == replyId && !r.IsDeleted);
            if (existing == null)
                return (false, "Reply not found");

            existing.Update(updatedBy, content);
            DB.SaveChanges();

            return (true, null);
        }

        // حذف رد
        public (bool, string) DeleteReply(int replyId, string deletedBy)
        {
            if (replyId <= 0 || string.IsNullOrEmpty(deletedBy))
            {
                return (false, "Invalid ReplyId or deletedBy");
            }

            var existing = DB.Reply.FirstOrDefault(r => r.Id == replyId && !r.IsDeleted);
            if (existing == null)
            {
                return (false, "Reply not found");
            }

            existing.Delete(deletedBy);
            DB.SaveChanges();
            return (true, null);
        }

        // جلب كل الردود الخاصة بتعليق معين
        public (bool, string, List<Reply>) GetAllReplies(int parentCommentId)
        {
            var replies = DB.Reply
                            .Where(r => r.ParentCommentID == parentCommentId && !r.IsDeleted)
                            .ToList();

            if (!replies.Any())
            {
                return (false, "There are no replies yet", null);
            }

            return (true, null, replies);
        }
    }
}
