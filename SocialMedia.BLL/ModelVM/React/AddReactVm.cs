namespace SocialMedia.BLL.ModelVM.React
{
    public class AddReactVm
    {
        public reactType Type { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public int PostID { get; set; }
    }
}
