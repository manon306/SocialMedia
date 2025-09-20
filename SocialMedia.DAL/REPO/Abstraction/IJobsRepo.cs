using SocialMedia.DAL.Entity;

namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IJobsRepo
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

