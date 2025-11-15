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
      
        public static IMappingExpression<TRole, TDto> IncludeBaseSelectRole< TRole,TDto>(this IMappingExpression<TRole, TDto> mapper)
        {
            return mapper.IncludeBase<AbpRoleBase,RoleSelectDto>();
        }
        
    }
}