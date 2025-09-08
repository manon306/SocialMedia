namespace SocialMedia.DAL.Entity
{
    public class React
    {
        //Properties
        public int ID { get; set; }
        public string Type { get; set; }
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

        //Methods
        public void Update(string UpdatedBy, string rection)
        {
            if (!string.IsNullOrEmpty(rection)) this.Type = rection;
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

