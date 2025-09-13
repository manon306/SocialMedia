using Microsoft.AspNetCore.Identity;

namespace SocialMedia.BLL.Service.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostsRepo postsRepo;
        private readonly IMapper mapper;
        public PostService(IPostsRepo postsRepo, IMapper mapper)
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
            // ??? ???????
            List<string>? imagePaths = null;
            List<string>? videoPaths = null;
            if (post.Image != null && post.Image.Count > 0)
            {
                imagePaths = Upload.UploadFile("Images", post.Image);
            }

            if (post.Videos != null && post.Videos.Count > 0)
            {
                videoPaths = Upload.UploadFile("Videos", post.Videos);
            }
            // Mapping
            var postEntity = new Post(post.Content, imagePaths, videoPaths,post.UserId,post.CreatedBy);
            if (postEntity == null)
            {
                return (false, "Mapping failed");
            }
            //use repo to add post
            var result = postsRepo.AddPost(postEntity);
            if (!result.Item1)
            {
                return (false, result.Item2);
            }
            //return result
            return (true, null);

        }
        public (bool, string, PostVm) SharePost(int postId, string userId, string? content)
        {
            if (postId <= 0 || string.IsNullOrEmpty(userId))
            {
                return (false, "Invalid postId or userId", null);
            }

            var result = postsRepo.SharePost(postId, userId, content);
            if (!result.Item1)
            {
                return (false, result.Item2, null);
            }

            // ????? ??????? ??? ViewModel
            var sharedPostVm = mapper.Map<PostVm>(result.Item3);
            return (true, null, sharedPostVm);
        }
        public (bool, string) DeletePost(int postId, string deletedBy)
        {
            //validation
            if (postId <= 0 || string.IsNullOrEmpty(deletedBy))
            {
                return (false, "Invalid postId or deletedBy");
            }
            //use repo to delete post
            var result = postsRepo.DeletePost(postId, deletedBy);
            if (!result.Item1)
            {
                return (false, result.Item2);
            }
            //return result
            return (true, null);
        }
        public (bool, string) UpdatePost(updatePostVm post)
        {
            //validation
            if (post == null)
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
        public (bool , string ,Post) GetById(int id)
        {
            if (id <= 0)
            {
                return (false, "Invalid PostId",null);
            }
            var result = postsRepo.GetPostById(id);
            if(result.Item1 == false)
            {
                return (false , result.Item2 , null);  
            }
            return (true, null, result.Item3);
        }
        public (bool, string, List<PostVm>) GetPosts()
        {
            //use repo to get posts
            var result = postsRepo.GetPosts();
            if (!result.Item1)
            {
                return (false, result.Item2, null);
            }

            //mapping
            var postsVm = mapper.Map<List<PostVm>>(result.Item3);

            // ???? ?? ?? ????? Shares ????? ???? ????
            foreach (var postVm in postsVm)
            {
                var postEntity = result.Item3.FirstOrDefault(p => p.ID == postVm.ID);
                if (postEntity != null && postEntity.Shares != null)
                {
                    postVm.CreatedByUserName = postEntity.User.UserName;
                    postVm.Shares = mapper.Map<ICollection<Share>>(postEntity.Shares);
                }
            }

            if (postsVm == null || postsVm.Count == 0)
            {
                return (false, "Mapping failed or no posts found", null);
            }

            //return result
            return (true, null, postsVm);
        }
        public (bool, string, List<PostVm>) GetSavedPosts()
        {
            //use repo to get posts
            var result = postsRepo.GetSavedPosts();
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
        public (bool, string) toggleSaved(int PostId)
        {
            if (PostId <= 0)
            {
                return (false, "Invalid PostId");
            }
            var result = postsRepo.ToggleSavePost(PostId);
            if (!result.Item1)
            {
                return (false, result.Item2);
            }
            return (true, null);
        }
        public (bool, string) toggleArchive(int PostId)
        {
            if (PostId <= 0)
            {
                return (false, "Invalid PostId");
            }
            var result = postsRepo.ToggleArchievePost(PostId);
            if (!result.Item1)
            {
                return (false, result.Item2);
            }

            
            return (true, null);
        }
        public void UnArchiveAllPosts()
        {
            
            postsRepo.unArchive();
        }
        public void UseHangfire()
        {
            RecurringJob.AddOrUpdate(() => UnArchiveAllPosts(),
                                            Cron.Monthly);
        }


    }
}
    

