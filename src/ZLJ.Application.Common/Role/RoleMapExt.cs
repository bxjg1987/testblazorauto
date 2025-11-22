using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.Roles;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Roles;

namespace AutoMapper
{
    public static class RoleMapCommonExt
    {
        public static IMappingExpression<TRole, TDto> IncludeBaseRoleSelectCommon< TRole,TDto>(this IMappingExpression<TRole, TDto> mapper)
        {
            return mapper.IncludeBase<Role, ZLJ.Application.Common.Share.Roles.RoleForSelectDto>();
        }
        public static IMappingExpression<TRole, TDto> IncludeBaseRoleCommon<TRole, TDto>(this IMappingExpression<TRole, TDto> mapper)
        {
            return mapper.IncludeBase<Role,ZLJ.Application.Common.Share.Roles. RoleDto>();
        }
        public static IMappingExpression< TDto, TRole> IncludeBaseRoleCreateCommon<TDto, TRole>(this IMappingExpression<TDto, TRole> mapper)
        {
            return mapper.IncludeBase<ZLJ.Application.Common.Share.Roles.RoleCreateDto, Role>();
        }
        public static IMappingExpression<TDto, TRole> IncludeBaseRoleEditCommon<TDto, TRole>(this IMappingExpression<TDto, TRole> mapper)
        {
            return mapper.IncludeBase<ZLJ.Application.Common.Share.Roles.RoleEditDto, Role>();
        }
    }
}