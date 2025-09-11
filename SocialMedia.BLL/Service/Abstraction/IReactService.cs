namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IReactService
    {
        (bool, string) AddReact(AddReactVm addReactVm);
        (bool, string) ToggleReact(AddReactVm reactVm);
        (bool, string, int) GetReactsCount(int postId);
        (bool, string, Dictionary<reactType, int>) GetReactsSummary(int postId);
        (bool, string, reactType?) GetUserReactType(int postId, string userId);
    }
}