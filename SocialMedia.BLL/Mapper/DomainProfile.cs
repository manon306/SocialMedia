using AutoMapper;
using SocialMedia.BLL.ModelVM.Comment;
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

        }


    }
}
