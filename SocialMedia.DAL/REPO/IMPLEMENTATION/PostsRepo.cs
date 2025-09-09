namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class PostsRepo : IPostsRepo
    {
        private readonly SocialMediaDbContext DB;
        public PostsRepo(SocialMediaDbContext DB)
        {
            this.DB = DB;
        }
        public (bool, string) AddPost(Post post)
        {
            
            if(post == null)
            {
                return (false, "Post cannot be null");
            }
            DB.Posts.Add(post);
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string) DeletePost(int postId, string deletedBy)
        {
            if(postId <= 0 || string.IsNullOrEmpty(deletedBy))
            {
                return (false, "Invalid postId or deletedBy");
            }
            var post = DB.Posts.FirstOrDefault(p=>p.ID == postId);
            post.Delete(deletedBy);
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string) UpdatePost(Post post)
        {
            if(post == null)
            {
                return (false, "Post cannot be null");
            }
            DB.Posts.Update(post);
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string, Post) GetPostById(int postId)
        {
            try
            {
                var post = DB.Posts
                             .FirstOrDefault(p => p.ID == postId && !p.IsDeleted);
                if (post == null)
                    return (false, "Post not found", null);
                return (true, null, post);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }
        private (bool, string, List<Post>) GetFilteredPosts(Expression<Func<Post, bool>> filter, string emptyMessage)
        {
            try
            {
                var posts = DB.Posts
                              .Where(p => !p.IsDeleted)  
                              .Where(filter)
                              .OrderByDescending(p => p.ID)
                              .ToList();

                if (!posts.Any())
                    return (false, emptyMessage, new List<Post>());

                return (true, null, posts);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", new List<Post>());
            }
        }
        public (bool , string ) ToggleSavePost(int postId)
        {
            if (postId <= 0)
            {
                return (false, "Invalid postId");
            }
            var post = DB.Posts.FirstOrDefault(p => p.ID == postId && !p.IsDeleted);
            if (post == null)
            {
                return (false, "Post not found");
            }
            post.ToggleSave();
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string) ToggleArchievePost(int postId)
        {
            if (postId <= 0)
            {
                return (false, "Invalid postId");
            }
            var post = DB.Posts.FirstOrDefault(p => p.ID == postId && !p.IsDeleted);
            if (post == null)
            {
                return (false, "Post not found");
            }
            post.ToggleArchive();
            DB.SaveChanges();
            return (true, null);
        }
        public (bool, string,List<Post>) unArchive()
        {
            var post = DB.Posts.Where(x => x.IsArchived == true).ToList();
            if (post == null)
            {
                return (false, "Post not found", null);
            }
            foreach (var p in post)
                p.unArchive();
            DB.SaveChanges();
            return (true, null, post);
        }
        public (bool, string, List<Post>) GetSavedPosts()
        {
            return GetFilteredPosts(p => p.IsSaved, "No saved posts found");
        }
        public (bool, string, List<Post>) GetArchivedPosts()
        {
            return GetFilteredPosts(p => p.IsArchived, "No archived posts found");
        }
        public (bool, string, List<Post>) GetPosts()
        {
            return GetFilteredPosts(p => true, "No posts found"); // كل البوستات غير المحذوفة
        }

    }
}
