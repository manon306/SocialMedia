


namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<User, ViewProfile>().ReverseMap();
            CreateMap<User, UserVM>().ForMember(f => f.PostBody, opt => opt.MapFrom(src => src.Post.Select(a => a.Body))).ReverseMap();

        }
    }
}
