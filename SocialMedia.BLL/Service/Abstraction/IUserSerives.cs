using SocialMedia.BLL.ModelVM.Profile;


namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IUserSerives
    {
        (bool, string, List<ViewProfileVM>) Search(string keyword);
        (bool, string, List<ViewProfileVM>) GetAll();
    }
}