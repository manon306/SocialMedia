namespace SocialMedia.BLL.ModelVM.Post
{
    public class PostVm
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public string? Videos { get; set; }
        public ICollection<Share> Shares { get; set; } = new List<Share>();
        public List<SocialMedia.DAL.Entity.Comment> Comments { get;  set; } = new List<SocialMedia.DAL.Entity.Comment>();
        public List<React> Reacts { get;  set; }
    }
}
