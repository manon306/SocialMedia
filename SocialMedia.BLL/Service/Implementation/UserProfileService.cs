using SocialMedia.BLL.ModelVM.Profile;
using SocialMedia.DAL.REPO.Abstraction;
using SocialMedia.DAL.REPO.IMPLEMENTATION;

namespace SocialMedia.BLL.Service.Implementation
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileRepo repo;
        private readonly IMapper mapper;

        public UserProfileService(IUserProfileRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<(bool success, string error)> CreateProfile(CreateProfileVM model, string userId)
        {
            try
            {
                string? imagePath = null;
                if (model.ProfileImage != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                    var path = Path.Combine("wwwroot/Files", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }

                var profile = new UserProfile
                {

                    //UserId = userId,
                    //Name = model.Name,
                    //Headline = model.Headline,
                    //Bio = model.Bio,
                    //Location = model.Location,
                    //Skills = model.Skills,
                    //ProfileImagePath = imagePath
                    UserId = userId,
                    Name = model.Name,
                    Headline = model.Headline,
                    Bio = model.Bio,
                    Location = model.Location,
                    Skills = string.IsNullOrEmpty(model.Skills) ? "N/A" : model.Skills,
                    ProfileImagePath = imagePath,
                    Education=model.Education,
                    Language=model.Language
                };

                await repo.Create(profile);
                return (true, null);
            }
            catch (Exception ex)
            {
                //return (false, ex.Message);
                return (false, ex.ToString());
            }
        }

        public async Task<ViewProfileVM?> GetProfile(string userId)
        {
            var profile = await repo.GetByUserId(userId);
            return mapper.Map<ViewProfileVM?>(profile);
        }

        public async Task<(bool success, string error)> EditProfile(EditProfileVM model, string userId)
        {
            try
            {
                var profile = await repo.GetByUserId(userId);
                if (profile == null)
                    return (false, "Profile not found");

                // Update fields
                profile.Name = model.Name;
                profile.Headline = model.Headline;
                profile.Bio = model.Bio;
                profile.Location = model.Location;
                profile.Skills = model.Skills;
                profile.Language = model.Language;
                profile.Education = model.Education;

                // Handle image upload (optional)
                if (model.ProfileImage != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                    var path = Path.Combine("wwwroot/Files", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }

                    profile.ProfileImagePath = fileName;
                }

                await repo.Update(profile);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public async Task<(bool success, string error)> DeleteProfile(string userId)
        {
            try
            {
                var profile = await repo.GetByUserId(userId);
                if (profile == null)
                    return (false, "Profile not found");

                await repo.Delete(profile);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }




    }
}
