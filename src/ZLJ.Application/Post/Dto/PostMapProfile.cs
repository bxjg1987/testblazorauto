using System.Linq;
using AutoMapper;
using Abp.Authorization;
using Abp.Authorization.Roles;
using ZLJ.Authorization.Roles;
using ZLJ.BaseInfo.Post;

namespace ZLJ.App.Admin.Post.Dto
{
    public class PostMapProfile : Profile
    {
        public PostMapProfile()
        {
            CreateMap<CreatePostDto, PostEntity>().IncludeBase<ZLJ.App.Admin.Roles.Dto.CreateRoleDto, Role>();
            CreateMap<Role, PostDto>().IncludeBase<Role, ZLJ.App.Admin.Roles.Dto.RoleDto>();
            CreateMap<PostEntity, PostDto>().IncludeBase<Role, ZLJ.App.Admin.Roles.Dto.RoleDto>();
            //CreateMap<Role, PostEditDto>().IncludeBase<Role, Roles.Dto.RoleEditDto>(); ;
        }
    }
}
