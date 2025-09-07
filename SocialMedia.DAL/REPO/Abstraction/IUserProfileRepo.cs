using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IUserProfileRepo
    {
        //void Create(UserProfile profile);
        //UserProfile GetByUserId(string userId);

        Task Create(UserProfile profile);
        Task<UserProfile> GetByUserId(string userId);

    }
}
