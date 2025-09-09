namespace SocialMedia.BLL.ModelVM.Comment
{
    public class AddReplyVm
    {
        [Required(ErrorMessage = "u must add content:(")]
        public string Content { get; set; }
        public int ParentCommentID { get; set; }
        public string? CreatedBy { get; set; }
    }
}
