namespace SocialMedia.DAL.Entity
{
    public class Comment
    {
        //constractor

        public Comment(string content, int postID, string createdById)
        {
            this.Content = content;
            this.PostID = postID;
            this.CreatedById = createdById;
            CreatedAt = DateTime.Now;
            IsDeleted = false;
        }



        //Properties

        public int ID { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public string? CreatedById { get; set; }   // مفتاح أجنبي (FK)
        public virtual User? CreatedBy { get; set; } // علاقة باليوزر

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
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

