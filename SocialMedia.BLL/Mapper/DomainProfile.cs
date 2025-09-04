using AutoMapper;
using SocialMedia.BLL.ModelVM.Comment;

namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Post, PostVm>().ReverseMap();
            CreateMap<Comment , UpdateCommentVm>().ReverseMap();
            CreateMap<Comment, GetCommentVm>().ReverseMap();
        }
    }
}
