namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IJobsRepo
    {
        Task<List<Job>> GetAllAsync();
        Task<Job?> GetByIdAsync(int id);
        Task ToggleSaveAsync(int id);
        Task UpdateReviewAsync(int id, string? review);
        Task<List<Job>> GetSavedAsync();
    }
}

