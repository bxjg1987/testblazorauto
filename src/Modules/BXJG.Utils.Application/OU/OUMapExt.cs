using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Organizations;
using AutoMapper;
using BXJG.Utils.Application.Share.OU;
using BXJG.Utils.Application.Share.Roles;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class OUMapExt
    {
        public static IMappingExpression<OrganizationUnit, OUSelectDto<TDto>> CreateMapSelectOu<TDto>(this Profile mapper) where TDto : OUSelectDto<TDto>
        {
            return mapper.CreateMap<OrganizationUnit, OUSelectDto<TDto>>();
        }
        public static IMappingExpression<OrganizationUnit, OUDto<TDto>> CreateMapOu<TDto>(this Profile mapper) where TDto : OUDto<TDto>
        {
            return mapper.CreateMap<OrganizationUnit, OUDto<TDto>>();
        }


        public static IMappingExpression<TEntity, TDto> IncludeBaseSelectOu<TEntity, TDto>(this IMappingExpression<TEntity, TDto> mapper) where TDto : OUSelectDto<TDto>
        {
            return mapper.IncludeBase<OrganizationUnit, OUSelectDto<TDto>>();
        }
        public static IMappingExpression<TEntity, TDto> IncludeBaseOuDto<TEntity, TDto>(this IMappingExpression<TEntity, TDto> mapper) where TDto : OUDto<TDto>
        {
            return mapper.IncludeBase<OrganizationUnit, OUDto<TDto>>();
        }


        public static IMappingExpression<TDto, TEntity> IncludeBaseOuEdit<TDto, TEntity>(this IMappingExpression<TDto, TEntity> mapper)
        {
            return mapper.IncludeBase<OUEditDto, OrganizationUnit>();
        }
        public static IMappingExpression<TDto, TEntity> IncludeBaseOuCreate<TDto, TEntity>(this IMappingExpression<TDto, TEntity> mapper)
        {
            return mapper.IncludeBase<OUCreateDto, OrganizationUnit>();
        }
    }
}