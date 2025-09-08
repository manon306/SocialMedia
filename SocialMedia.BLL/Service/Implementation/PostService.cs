using SocialMedia.BLL.Service.Abstraction;

namespace SocialMedia.BLL.Service.Implementation
{
    public class PostService : IPostService
    {
        public void UseHangfire()
        {
            // Hangfire is disabled in development
            // This method exists to prevent compilation errors
        }
    }
}
