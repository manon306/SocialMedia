using AutoMapper;
using SocialMedia.BLL.ModelVM.Comment;
using SocialMedia.BLL.ModelVM.Connect;
using SocialMedia.BLL.ModelVM.Profile;
using SocialMedia.BLL.ModelVM.User;

namespace SocialMedia.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Post, PostVm>().ReverseMap();
            CreateMap<Comment , UpdateCommentVm>().ReverseMap();
            CreateMap<Comment, GetCommentVm>().ReverseMap();
            CreateMap<Reply, GetCommentVm>().ReverseMap();
            CreateMap<UpdateCommentVm, Comment>();
            CreateMap<Reply , AddReplyVm>().ReverseMap();
            CreateMap<User, ViewProfileVM>().ReverseMap();
            CreateMap<User, UserVM>().ForMember(f => f.PostBody, opt => opt.MapFrom(src => src.Post.Select(a => a.Content))).ReverseMap();
            // UserProfile -> ViewProfileVM
            CreateMap<UserProfile, ViewProfileVM>();

            // CreateProfileVM -> UserProfile
            CreateMap<CreateProfileVM, UserProfile>();

            CreateMap<User, FriendVM>()
            .ForMember(d => d.ImagePath, o => o.MapFrom(s => s.ImagePath))
            .ForMember(d => d.Headline, o => o.MapFrom(s => s.Headline));

            CreateMap<Connection, ConnectionRequestVM>()
                .ForMember(d => d.SenderName, o => o.MapFrom(s => s.Sender.Name))
                .ForMember(d => d.SenderHeadline, o => o.MapFrom(s => s.Sender.Headline))
                .ForMember(d => d.SenderImage, o => o.MapFrom(s => s.Sender.ImagePath));
        }


    }
}

