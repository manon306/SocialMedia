
using SocialMedia.BLL.ModelVM.Connect;
using SocialMedia.BLL.ModelVM.Profile;
using SocialMedia.BLL.ModelVM.User;



namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile  : Profile
    {

        public DomainProfile()
        {
            CreateMap<User, ViewProfileVM>().ReverseMap();
            CreateMap<User, UserVM>().ForMember(f => f.PostBody, opt => opt.MapFrom(src => src.Post.Select(a => a.Body))).ReverseMap();


            // UserProfile -> ViewProfileVM
            CreateMap<UserProfile, ViewProfileVM>();

    //        CreateMap<User, ViewProfileVM>()
    //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) 
    //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    //.ForMember(dest => dest.Headline, opt => opt.MapFrom(src => src.Headline))
    //.ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
    //.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
    //.ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.UserProfile.Skills))
    //.ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.UserProfile.ProfileImagePath));

            // CreateProfileVM -> UserProfile
            CreateMap<CreateProfileVM, UserProfile>();



            CreateMap<User, FriendVM>()
    .ForMember(d => d.ProfileImagePath, o => o.MapFrom(s => s.ImagePath))
    .ForMember(d => d.Headline, o => o.MapFrom(s => s.Headline));
           


            CreateMap<Connection, ConnectionRequestVM>()
                .ForMember(d => d.SenderName, o => o.MapFrom(s => s.Sender.Name))
                .ForMember(d => d.SenderHeadline, o => o.MapFrom(s => s.Sender.Headline))
                .ForMember(d => d.SenderImage, o => o.MapFrom(s => s.Sender.ImagePath));
        }




    }
}
