using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Common.Role
{
    public class ObjectMapProfile : Profile
    {
        public ObjectMapProfile()
        {
            CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleDto>().IncludeBaseRole();
            CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleForSelectDto>().IncludeBaseRoleSelect();
            CreateMap<RoleEditDto, ZLJ.Core.Authorization.Roles.Role >().IncludeBaseRoleEdit();
            CreateMap<RoleCreateDto, ZLJ.Core.Authorization.Roles.Role >().IncludeBaseRoleCreate();
        }
    }
}
