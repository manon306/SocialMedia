namespace SocialMedia.PL.Models
{
    public class ProfileEditViewModel
    {
        public string Id { get; set; } = null!;
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}