using AutoMapper;
using SocialMedia.BLL.ModelVM.User;
using SocialMedia.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
