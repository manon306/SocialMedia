using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IFollowSerives
    {
        Task FollowUser(string followerId, string followingId);
        Task UnfollowUser(string followerId, string followingId);
    }
}
