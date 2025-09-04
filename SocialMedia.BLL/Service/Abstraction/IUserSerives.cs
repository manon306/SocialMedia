using SocialMedia.BLL.ModelVM.User;


namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IUserSerives
    {
        (bool, string) Create(CreateProfile user);
        (bool, string,List<ViewProfile>) ViewProfile();
        
    }
}
