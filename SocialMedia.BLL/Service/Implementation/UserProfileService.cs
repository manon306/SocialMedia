using SocialMedia.BLL.ModelVM.Profile;


namespace SocialMedia.BLL.Service.Implementation
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileRepo _repo;
        private readonly IMapper _mapper;

        public UserProfileService(IUserProfileRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
                    UserId = userId,
                    Name = model.Name,
                    Headline = model.Headline,
                    Bio = model.Bio,
                    Location = model.Location,
                    Skills = model.Skills,
                    ProfileImagePath = imagePath
                };

                await _repo.Create(profile);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<ViewProfileVM?> GetProfile(string userId)
        {
            var profile = await _repo.GetByUserId(userId);
            return _mapper.Map<ViewProfileVM?>(profile);
        }
    }
}
