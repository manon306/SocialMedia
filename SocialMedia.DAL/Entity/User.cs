using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using SocialMedia.DAL.Entity;

namespace SocialMedia.DAL.Entity
{
    public class User:IdentityUser
    {
        public User()
        {

        }
        public User(string? name, string?bio, string? createdBy, string? imagePath)
        {
            Name = name;
            Bio = bio;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            ImagePath = imagePath;

          
        }

        
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }           
        public string? Headline { get; set; }   
        public string? Location { get; set; }
        public string? ImagePath { get; set; }
        public string Education { get; set; }
        public string Language { get; set; }

        public UserProfile UserProfile { get; set; }

        public string? CreatedBy { get; private set; }//Identity
        public DateTime? CreatedOn { get; private set; }
        public bool? IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? ModifiedBy { get; private set; }//Identity
        public DateTime? ModifiedOn { get; private set; }

        public ICollection<Share> Shares { get; set; } = new List<Share>();

        public int ConnectionsCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }


         public virtual List<Post> Post { get; private set; }

        public bool Update(string modifierUser, string name, int age)
        {
            if (string.IsNullOrEmpty(modifierUser))
                return false;
            Name = name;
            Age = age;
            ModifiedOn = DateTime.Now;
            ModifiedBy = modifierUser;
            return true;
        }

        internal bool ToggelStatus(string v)
        {
            throw new NotImplementedException();
        }
        //public bool ToggelStatus(string DeletedUser)
        //{

        //    if (string.IsNullOrEmpty(DeletedUser))
        //        return false;
        //    IsDeleted = !IsDeleted;
        //    DeletedOn = DateTime.Now;
        //    return true;
        //}
    }
}
