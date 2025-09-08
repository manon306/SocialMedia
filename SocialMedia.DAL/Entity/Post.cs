namespace SocialMedia.DAL.Entity
{
    public class Post
    {
        public Post(string Content, string? Image, string? Videos)
        {
            this.Content = Content;
            this.Image = Image;
            this.Videos = Videos;
            this.IsDeleted = false;
            this.CreatedAt = DateTime.Now;
            this.IsArchived = false;
            this.IsSaved = false;
            this.CreatedBy = "Menna";
            this.Comments = new List<Comment>();
            this.Reacts = new List<React>();
            this.UserID = 1;
        }

        public int ID { get; private set; }
        public string Content { get; private set; }
        public string? Image { get; private set; }
        public string? Videos { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public string CreatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsSaved { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public int UserID { get; private set; }

        public List<Comment> Comments { get; private set; }
        public List<React> Reacts { get; private set; }

        public void Update(string UpdatedBy, string content, string? Image, string? Videos)
        {
            if (!string.IsNullOrEmpty(content)) this.Content = content;
            if (!string.IsNullOrEmpty(Image)) this.Image = Image;
            if (!string.IsNullOrEmpty(Videos)) this.Videos = Videos;
            this.UpdatedAt = DateTime.Now;
            this.UpdatedBy = UpdatedBy;
        }
        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            this.DeletedAt = DateTime.Now;
            this.DeletedBy = deletedBy;
        }
        public void ToggleArchive()
        {
            this.IsArchived = !this.IsArchived;
        }
        public void unArchive()
        {
            this.IsArchived = false;
        }
        public void ToggleSave()
        {
            this.IsSaved = !this.IsSaved;
        }
    }
}

