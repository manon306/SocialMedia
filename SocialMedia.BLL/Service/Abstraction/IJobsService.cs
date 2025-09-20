using SocialMedia.DAL.Entity;

namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IJobsService
    {
        Task<List<Job>> GetAllAsync();
        Task<Job?> GetByIdAsync(int id);
        Task ToggleSaveAsync(int id);
        Task UpdateReviewAsync(int id, string? review);
        Task<List<Job>> GetSavedAsync();
        Task<Job> CreateAsync(Job job);
        Task<Job> UpdateAsync(Job job);
        Task DeleteAsync(int id);
    }
}

