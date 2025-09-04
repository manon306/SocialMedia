using SocialMedia.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IuserRepo
    {
        bool Create(User user);
      
        List<User> GetUsers();
    }
}
