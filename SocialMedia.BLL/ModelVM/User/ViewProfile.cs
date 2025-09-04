using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.ModelVM.User
{
    public class ViewProfile
    {
        public string Name { get; set; }
        public string? Headline { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }

        
        /// /
       //
        public List<string> Skills { get; set; }

        public IFormFile PersonalImag { get; set; }
    }
}
