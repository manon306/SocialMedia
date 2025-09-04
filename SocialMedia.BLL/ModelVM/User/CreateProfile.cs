

namespace SocialMedia.BLL.ModelVM.User
{
    public class CreateProfile
    {
        public string Name { get; set; }
        public string? Headline { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }

        public List<string> Skills { get; set; }

        public IFormFile PersonalImag { get; set; }




    }
}
