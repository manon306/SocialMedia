namespace SocialMedia.BLL.ModelVM.Job
{
    public class JobVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsSaved { get; set; }
        public string? Review { get; set; }
    }
}
