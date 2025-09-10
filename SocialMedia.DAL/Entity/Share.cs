namespace SocialMedia.DAL.Entity
{
    public class Share
    {
        public int ID { get; private set; }
        public int PostId { get; private set; }
        public Post Post { get; set; }

        public string UserId { get; private set; }
        public User User { get; set; }

        public DateTime SharedAt { get; set; } = DateTime.Now;
        public string? Content { get; set; }

        // Constructor
        public Share() { }
        public Share(string? content, int postId, string Userid)
        {
            Content = content;
            PostId = postId;
            UserId = Userid;
            SharedAt = DateTime.Now;
        }

        // Methods
        public void UpdateContent(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                Content = content;
            }
        }
    }
}
