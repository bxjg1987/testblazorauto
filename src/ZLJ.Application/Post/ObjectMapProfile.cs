using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Application.Share.Post;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;



namespace ZLJ.Application.Post
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            CreateMap<PostEntity, PostDto>();//.IncludeBaseRoleCommon();
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleForSelectDto>().IncludeBaseRoleSelect();
            CreateMap<PostEditDto, PostEntity>().IncludeBaseRoleEditCommon();
            CreateMap<PostCreateDto, PostEntity>();//.IncludeBaseRoleCreateCommon();
        }
    }
}
