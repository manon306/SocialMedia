namespace SocialMedia.BLL.ModelVM.Comment
{
    public class UpdateCommentVm
    {
        public int ID { get;  set; }
        public string Content { get; set; }
        public int PostID { get; set; }
        public string? UpdatedBy { get;  set; }
    }
}
