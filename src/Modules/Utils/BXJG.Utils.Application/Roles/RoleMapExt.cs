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

namespace AutoMapper
{
    public static class RoleMapExt
    {
        public static IMappingExpression<TRole, TDto> IncludeBaseRoleSelect< TRole,TDto>(this IMappingExpression<TRole, TDto> mapper)
        {
            return mapper.IncludeBase<AbpRoleBase,RoleSelectDto>();
        }
        public static IMappingExpression<TRole, TDto> IncludeBaseRole<TRole, TDto>(this IMappingExpression<TRole, TDto> mapper)
        {
            return mapper.IncludeBase<AbpRoleBase, RoleDto>();
        }
        public static IMappingExpression< TDto, TRole> IncludeBaseRoleCreate<TDto, TRole>(this IMappingExpression<TDto, TRole> mapper)
        {
            return mapper.IncludeBase< RoleCreateDto,AbpRoleBase>();
        }
        public static IMappingExpression<TDto, TRole> IncludeBaseRoleEdit<TDto, TRole>(this IMappingExpression<TDto, TRole> mapper)
        {
            return mapper.IncludeBase<RoleEditDto, AbpRoleBase>();
        }
    }
}