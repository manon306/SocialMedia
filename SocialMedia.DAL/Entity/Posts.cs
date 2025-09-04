


namespace SocialMedia.DAL.Entity
{
    public class Posts
    {
        public int Id { get; set; }
        public string Body { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
