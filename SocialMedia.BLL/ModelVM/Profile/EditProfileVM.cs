using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.ModelVM.Profile
{
   
        public class EditProfileVM
        {
            public string Name { get; set; }
            public string Headline { get; set; }
            public string Bio { get; set; }
            public string Location { get; set; }
            public string Skills { get; set; }
        public string Education { get; set; }
        public string Language { get; set; }
        public IFormFile? ProfileImage { get; set; }
       
    }
}
