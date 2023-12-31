using System.Linq;
using AutoMapper;
using Abp.Authorization;
using Abp.Authorization.Roles;
using ZLJ.Authorization.Roles;
using ZLJ.BaseInfo.Post;
using ZLJ.Application.Share.Post;
using ZLJ.Application.Share.Roles;

namespace ZLJ.App.Admin.Post.Dto
{
    public class PostMapProfile : Profile
    {
        public PostMapProfile()
        {
            CreateMap<CreatePostDto, PostEntity>().IncludeBase<CreateRoleDto, Role>();
            CreateMap<Role, PostDto>().IncludeBase<Role, RoleDto>();
            CreateMap<PostEntity, PostDto>().IncludeBase<Role, RoleDto>();
            //CreateMap<Role, PostEditDto>().IncludeBase<Role, Roles.Dto.RoleEditDto>(); ;
        }
    }
}
