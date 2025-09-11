namespace SocialMedia.DAL.Entity
{
    public class React
    {
        public React() { }
        public React(reactType type, string createdBy, int postID)
        {
            Type = type;
            CreatedBy = createdBy;
            IsDeleted = false;
            PostID = postID;
            CreatedAt = DateTime.Now;
        }

        //Properties
        public int ID { get; set; }
        public reactType Type { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public string? DeletedBy { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        //navigation properties
        public int PostID { get; set; }
        public virtual Post Post { get; set; }

        //Methods
        public void Update(string updatedBy, reactType reaction)
        {
            this.Type = reaction;
            this.UpdatedBy = updatedBy;
            this.UpdatedAt = DateTime.Now;
        }

        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            this.DeletedBy = deletedBy;
            this.DeletedAt = DateTime.Now;
        }

        public void Restore()
        {
            this.IsDeleted = false;
            this.DeletedBy = null;
            this.DeletedAt = null;
        }
    }
}