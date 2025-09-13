namespace SocialMedia.BLL.ModelVM.Post
{
    public class CreateVm
    {
        [Required(ErrorMessage ="This Field is required")]
        public string Content { get; set; }
        public List<IFormFile>? Image { get; set; }
        public List<IFormFile>? Videos { get; set; }
        public string UserId { get;  set; }
        public string CreatedBy { get; set; }
    }
}
