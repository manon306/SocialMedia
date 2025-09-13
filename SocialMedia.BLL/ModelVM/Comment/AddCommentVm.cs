namespace SocialMedia.BLL.ModelVM.Comment
{
    public class AddCommentVm
    {
        [Required(ErrorMessage ="u must add content:(")]
        public string Content { get; set; }
        public int PostID { get; set; }
        public string CreatedById { get; set; }
    }
}
