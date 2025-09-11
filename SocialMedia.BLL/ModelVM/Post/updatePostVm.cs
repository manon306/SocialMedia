namespace SocialMedia.BLL.ModelVM.Post
{
    public class updatePostVm
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public List<string>? Image { get; set; }
        public List<string>? Videos { get; set; }
        public string UpdatedBy { get; set; }
    }
}
