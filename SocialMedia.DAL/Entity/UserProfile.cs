using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SocialMedia.DAL.Entity
{

    [Table("UserProfiles")]
     
 
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }   // 
        //[Required]
        public string UserId { get; set; }  

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }   // one to one 

        [Required]
        public string Name { get; set; }
        public string Headline { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? Language { get; set; }
        public string? ProfileImagePath { get; set; }

        public UserProfile()
        {

        }
        public UserProfile(string? name, string? bio, string? headLine, string? location,string?skilks,  string? education, string? language ,string? profileImagePath)
        {
            Name = name;
            Bio = bio;
            Headline = headLine;
            Location = location;
            Skills = skilks;
            ProfileImagePath = profileImagePath;
            Education = education;
            Language = language;
        }


        

    }
}
