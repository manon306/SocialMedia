

namespace SocialMedia.DAL.Entity
{
    public class UserProfile
    {
        public string Id { get; set; }

        public string UserId { get; set; }  
        public User User { get; set; }     

        public string Name { get; set; }
        public string Headline { get; set; }   
        public string Bio { get; set; }
        public string Location { get; set; }

        public string Skills { get; set; }

        public string? ProfileImagePath { get; set; } 


    }
}
