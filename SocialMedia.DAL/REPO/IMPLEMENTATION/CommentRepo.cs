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
        public (bool, string) UpdateComment(Comment comment)
        {
            if (comment == null)
                return (false, "Comment cannot be null");

            var existing = DB.Comments.FirstOrDefault(c => c.ID == comment.ID && !c.IsDeleted);
            if (existing == null)
                return (false, "Comment not found");


            existing.Update(comment.UpdatedBy, comment.Content);
            DB.SaveChanges();

            return (true, null);
        }
        public (bool, string) DeletePost(int commentId ,string Deletedby)
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
    }
}
