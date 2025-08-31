using AutoMapper;

namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Post, PostVm>().ReverseMap();
        }
    }
}
