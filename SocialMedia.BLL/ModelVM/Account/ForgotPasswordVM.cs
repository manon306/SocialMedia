namespace SocialMedia.BLL.ModelVM.Account
{
    public class ForgotPasswordVM
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
    }
}