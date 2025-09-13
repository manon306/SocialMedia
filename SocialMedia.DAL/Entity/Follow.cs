namespace SocialMedia.DAL.Entity
{
    public class Follow
    {
        public int Id { get; set; }

        public string FollowerId { get; set; }
        public User Follower { get; set; }

        public string FollowingId { get; set; }
        public User Following { get; set; }

    }
}