using SocialMedia.BLL.ModelVM.job;

namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IJobsService
    {
        Task<List<Jobvm>> GetAllAsync();
        Task<Jobvm?> GetByIdAsync(int id);
        Task ToggleSaveAsync(int id);
        Task UpdateReviewAsync(int id, string? review);
        Task<List<Jobvm>> GetSavedAsync();
        Task AddAsync(Jobvm job);
        Task DeleteAsync(int id);
    }
}

