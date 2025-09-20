using AutoMapper;
using SocialMedia.BLL.ModelVM.Comment;
using SocialMedia.BLL.ModelVM.Job;
using SocialMedia.DAL.Entity;

namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Post, PostVm>().ReverseMap();
            CreateMap<Comment , UpdateCommentVm>().ReverseMap();
            CreateMap<Comment, GetCommentVm>().ReverseMap();
            CreateMap<Job, JobVm>().ReverseMap();
            CreateMap<Job, UpdateJobVm>().ReverseMap();
        }
    }
}
