namespace SocialMedia.DAL.Entity
{
    public class React
    {
        public int ID { get; private set; }
        public string Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public int PostID { get; private set; }
        public int? CommentID { get; private set; }
        public Post Post { get; private set; }
        public Comment? Comment { get; private set; }
    }
}

