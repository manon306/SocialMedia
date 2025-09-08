namespace SocialMedia.DAL.Entity
{
    public class Job
    {
        private Job() { }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Company { get; private set; }
        public string? Location { get; private set; }
        public string? Description { get; private set; }
        public DateTime PostedAt { get; private set; } = DateTime.Now;
        public bool IsSaved { get; private set; }
        public string? Review { get; private set; }

        public Job(string title, string company, string? location, string? description)
        {
            Title = title;
            Company = company;
            Location = location;
            Description = description;
        }

        public void Update(string title, string company, string? location, string? description)
        {
            if (!string.IsNullOrWhiteSpace(title)) Title = title;
            if (!string.IsNullOrWhiteSpace(company)) Company = company;
            Location = location;
            Description = description;
        }

        public void ToggleSave()
        {
            IsSaved = !IsSaved;
        }

        public void SetReview(string? review)
        {
            Review = review;
        }
    }
}

