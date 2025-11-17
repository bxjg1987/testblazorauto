using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;

namespace ZLJ.Application.Common.Post
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleDto>().IncludeBaseRole();
            CreateMap<PostEntity, PostForSelectDto>().IncludeBaseRoleSelectCommon();
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleEditDto>().IncludeBaseRoleEdit();
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleCreateDto>().IncludeBaseRoleCreate();
        }
    }
}
