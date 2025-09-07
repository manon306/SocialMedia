using SocialMedia.BLL.ModelVM.Profile;


namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IUserProfileService
    {
       Task< (bool success, string error)> CreateProfile(CreateProfileVM model, string userId);
       Task< ViewProfileVM> GetProfile(string userId);
    }
}
