using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.ModelVM.User
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string? Headline { get; set; }
        public string? Bio { get; set; }
        public string? ImagePath { get; set; }
        public string? Location { get; set; }

        public List<string> PostBody { get; set; }
        public List<string> Skills { get; set; }
        public int Age { get; set; }
        public int ConnectionsCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
