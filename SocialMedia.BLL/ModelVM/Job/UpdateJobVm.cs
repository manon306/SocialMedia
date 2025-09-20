using System.ComponentModel.DataAnnotations;

namespace SocialMedia.BLL.ModelVM.Job
{
    public class UpdateJobVm
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string Company { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }
    }
}
