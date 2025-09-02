using System.ComponentModel.DataAnnotations;

namespace SocialMedia.BLL.ModelVM.Post
{
    public class CreateVm
    {
        [Required(ErrorMessage ="This Field is required")]
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Videos { get; set; }
    }
}
