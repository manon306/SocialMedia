namespace SocialMedia.BLL.ModelVM.job
{
    public class Jobvm
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Company { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;
        public string? Location { get; private set; }
    }
}
