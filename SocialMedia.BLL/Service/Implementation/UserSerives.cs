using AutoMapper;
using SocialMedia.BLL.Helper;
using SocialMedia.BLL.ModelVM.User;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SocialMedia.BLL.Service.Implementation
{
    public class UserSerives : IUserSerives
    {
        private readonly IuserRepo userRepo;
        private readonly IMapper mapper;
        

        public UserSerives(IMapper mapper, IuserRepo userRepo)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        public (bool, string) Create(CreateProfile user)
        {
            try
            {
                // Get Image Path in server 
                var imagePath = Upload.UploadFile("Files", user.PersonalImag);

                var mappuser = new User(user.Name, user.Bio, "System", imagePath);
                var result = userRepo.Create(mappuser);

                if (result)
                    return (true, null);

                return (false, "There was an error saving the user.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string, List<ViewProfile>) ViewProfile()
        {
            try
            {
                var users = userRepo.GetUsers();
                var result = mapper.Map<List<ViewProfile>>(users);

                return (true, null, result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
