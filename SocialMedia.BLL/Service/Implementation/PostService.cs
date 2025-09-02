using SocialMedia.BLL.Helper;

namespace SocialMedia.BLL.Service.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostsRepo postsRepo;
        private readonly IMapper mapper;
        public PostService(IPostsRepo postsRepo , IMapper mapper)
        {
            this.postsRepo = postsRepo;
            this.mapper = mapper;
        }
        public (bool, string) AddPost(CreateVm post)
        {
            
            //validation
            if (post == null)
            {
                return (false, "Post cannot be null");
            }// Upload files (store file name or path)
            string? imagePath = post.Image != null ? Upload.UploadFile("Images", post.Image) : null;
            string? videoPath = post.Videos != null ? Upload.UploadFile("Videos", post.Videos) : null;

            // Mapping
            var postEntity = new Post(post.Content, imagePath, videoPath);
            if (postEntity == null)
            {
                return (false, "Mapping failed");
            }
            //use repo to add post
            var result = postsRepo.AddPost(postEntity);
            if(!result.Item1)
            {
                return (false, result.Item2);
            }
            //return result
            return (true ,null);

        }
        public (bool, string) DeletePost(int postId, string deletedBy)
        {
            //validation
            if(postId <= 0 || string.IsNullOrEmpty(deletedBy))
            {
                return (false, "Invalid postId or deletedBy");
            }
            //use repo to delete post
            var result = postsRepo.DeletePost(postId, deletedBy);
            if(!result.Item1)
            {
                return (false, result.Item2);
            }
            //return result
            return (true ,null);
        }
        public (bool ,string) UpdatePost(updatePostVm post)
        {
            //validation
            if (post == null )
            {
                return (false, " no Post to update");
            }
            //use repo
            var getResult = postsRepo.GetPostById(post.ID);
            if (!getResult.Item1)
            {
                return (false, getResult.Item2);
            }
            var oldpost = getResult.Item3;
            //update fields
            oldpost.Update(post.UpdatedBy, post.Content, post.Image, post.Videos);
            //use Repo to save Changes
            var updateResult = postsRepo.UpdatePost(oldpost);
            if (!updateResult.Item1)
            {
                return (false, updateResult.Item2);
            }
            return (true, null);
        }
        public (bool, string, List<PostVm>) GetPosts()
        {
            //use repo to get posts
            var result = postsRepo.GetPosts();
            if(!result.Item1)
            {
                return (false, result.Item2,null);
            }
            //mapping
            var postsVm = mapper.Map<List<PostVm>>(result.Item3);
            if(postsVm == null || postsVm.Count == 0)
            {
                return (false, "Mapping failed or no posts found",null);
            }
            //return result
            return (true, null, postsVm);
        }
        public (bool, string, List<PostVm>) GetSavedPosts()
        {
            //use repo to get posts
            var result = postsRepo.GetSavedPosts();
            if(!result.Item1)
            {
                return (false, result.Item2, null);
            }
            var postVm =mapper.Map<List<PostVm>>(result.Item3);
            if (postVm == null || postVm.Count == 0)
            {
                return (false, "Mapping failed or no posts found", null);
            }
            //return result
            return (true, null, postVm);
        }
        public (bool, string, List<PostVm>) GetArchivedPosts()
        {
            //use repo to get posts
            var result = postsRepo.GetArchivedPosts();
            if (!result.Item1)
            {
                return (false, result.Item2, null);
            }
            var postVm = mapper.Map<List<PostVm>>(result.Item3);
            if (postVm == null || postVm.Count == 0)
            {
                return (false, "Mapping failed or no posts found", null);
            }
            //return result
            return (true, null, postVm);
        }

    }
}
