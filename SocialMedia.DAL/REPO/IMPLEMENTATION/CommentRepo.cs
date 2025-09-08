namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class CommentRepo : ICommentRepo
    {
        private readonly SocialMediaDbContext DB;
        public CommentRepo(SocialMediaDbContext DB) 
        { 
            this.DB = DB;
        }
        public (bool , string ) AddComment(Comment comment)
        {
            if( comment == null)
            {
                return (false, "comment is required");
            }
            DB.Comments.Add(comment);
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string) UpdateComment(int commentId,string Content , string Updatedby)
        {
            if (Content == null)
                return (false, "Comment cannot be null");

            var existing = DB.Comments.Where(a=>a.ID == commentId).FirstOrDefault();
            if (existing == null)
                return (false, "Comment not found");

            existing.Update(Updatedby,Content);

            DB.SaveChanges();

            return (true, null);
        }

        public (bool, string) DeleteComment(int commentId ,string Deletedby)
        {
            if(commentId< 0 || string.IsNullOrEmpty(Deletedby))
            {
                return (false, "Invalid CommentId or deletedBy");
            }
            var result = DB.Comments.FirstOrDefault(x=>x.ID==commentId);
            if (result == null)
            {
                return (false, "cannot be null");
            }
            result.Delete(Deletedby);
            DB.SaveChanges();
            return (true, null);
        }
        public (bool , string , List<Comment>) GetAllComments(int PostId)
        {
            var result = DB.Comments.Where(x=>x.PostID ==  PostId && x.IsDeleted == false).ToList();
            if (!result.Any())
            {
                return (false, "there is no comment yet", null);
            }
            return (true,null ,result);
        }
    }
}
