using Microsoft.EntityFrameworkCore;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;

namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class JobsRepo : IJobsRepo
    {
        private readonly SocialMediaDbContext _dbContext;

        public JobsRepo(SocialMediaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Job>> GetAllAsync()
        {
            return _dbContext.Jobs.AsNoTracking().OrderByDescending(j => j.PostedAt).ToListAsync();
        }

        public Task<Job?> GetByIdAsync(int id)
        {
            return _dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task ToggleSaveAsync(int id)
        {
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return;
            job.ToggleSave();
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(int id, string? review)
        {
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return;
            job.SetReview(review);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Job>> GetSavedAsync()
        {
            return _dbContext.Jobs.AsNoTracking().Where(j => j.IsSaved).OrderByDescending(j => j.PostedAt).ToListAsync();
        }
    }
}

