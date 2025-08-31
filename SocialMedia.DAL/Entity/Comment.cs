namespace SocialMedia.DAL.Entity
{
    public class Comment
    {
        //Properties
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //navigation properties
        public int PostID { get; set; }
        public virtual Post Post { get; set; }

        public List<React> Reacts { get; set; }

        //Methods
        public void Update(string UpdatedBy, string content)
        {
            if (!string.IsNullOrEmpty(content)) this.Content = content;
            this.UpdatedAt = DateTime.Now;
            this.UpdatedBy = UpdatedBy;
        }
        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            this.DeletedAt = DateTime.Now;
            this.DeletedBy = deletedBy;
        }
    }
}
