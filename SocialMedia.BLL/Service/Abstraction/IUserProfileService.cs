namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IUserProfileService
    {
        Task<(bool success, string error)> CreateProfile(CreateProfileVM model, string userId);
        //Task<ViewProfileVM> GetProfile();
        Task<ViewProfileVM?> GetProfile(string userId);
        Task<(bool success, string error)> EditProfile(EditProfileVM model, string userId);
        Task<(bool success, string error)> DeleteProfile(string userId);
    }
}