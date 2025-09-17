using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SocialMedia.DAL.Entity
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public User(string? name, string? bio, string? createdBy, string? imagePath)
        {
            Name = name;
            Bio = bio;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            ImagePath = imagePath;
        }

        // Existing properties
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }
        public string? Headline { get; set; }
        public string? Location { get; set; }
        public string? ImagePath { get; set; }

        public string? CreatedBy { get; private set; } // Identity
        public DateTime? CreatedOn { get; private set; }
        public bool? IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? ModifiedBy { get; private set; } // Identity
        public DateTime? ModifiedOn { get; private set; }

        public int ConnectionsCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }

        // ✅ New subscription flag
        public bool IsPremium { get; set; } = false;

        public virtual List<Post> Post { get; private set; }

        // New: Google Maps integration fields
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Methods
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

        // Example toggle (commented out in your version)
        //public bool ToggelStatus(string deletedUser)
        //{
        //    if (string.IsNullOrEmpty(deletedUser))
        //        return false;
        //    IsDeleted = !IsDeleted;
        //    DeletedOn = DateTime.Now;
        //    return true;
        //}
    }
}