using Abp.Authorization.Users;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class UserMapExt
    {
        ///// <summary>
        ///// EditUserDto映射到到AbpUser
        ///// </summary>
        ///// <typeparam name="TUser"></typeparam>
        ///// <param name="mapper"></param>
        ///// <returns></returns>
        //public static IMappingExpression<EditUserDto,AbpUserBase> IncludeBaseUser(this IMappingExpression<EditUserDto, AbpUserBase> mapper)
        //{
        //    return mapper.IncludeBase<EditUserDto, AbpUserBase>();
        //}

        /// <summary>
        /// EditUserDto映射到到AbpUser
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static IMappingExpression<UserEditDto, AbpUser<TUser>> CreateUserEditMap<TUser>(this IProfileExpression profile)
            where TUser : AbpUser<TUser>
        {
            return profile.CreateMap<UserEditDto, AbpUser<TUser>>().IncludeBase<UserEditDto, AbpUserBase>();
            //return mapper.IncludeBase<UserEditDto, AbpUser<TUser>>().IncludeBase<UserEditDto, AbpUserBase>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static IMappingExpression<UserCreateDto, AbpUser<TUser>> CreateUserCreateMap<TUser>(this IProfileExpression profile)
            where TUser : AbpUser<TUser>
        {
            return profile.CreateMap<UserCreateDto, AbpUser<TUser>>().IncludeBase<UserCreateDto, AbpUserBase>();
        }
        public static IMappingExpression<AbpUser<TUser>, UserSelectDto> CreateUserSelectMap<TUser>(this IProfileExpression profile)
           where TUser : AbpUser<TUser>
        {
            return profile.CreateMap<AbpUser<TUser>, UserSelectDto>().IncludeBase<AbpUserBase, UserSelectDto>();
        }
        public static IMappingExpression<AbpUser<TUser>, UserDto> CreateUserMap<TUser>(this IProfileExpression profile)
           where TUser : AbpUser<TUser>
        {
            return profile.CreateMap<AbpUser<TUser>, UserDto>().IncludeBase<AbpUserBase, UserDto>();
        }



        public static IMappingExpression<TDto, TUser> IncludeBaseEditUser<TDto, TUser>(this IMappingExpression<TDto, TUser> mapper)
            where TUser : AbpUser<TUser>
        {
            return mapper.IncludeBase<UserEditDto, AbpUser<TUser>>();
        }
        public static IMappingExpression<TDto, TUser> IncludeBaseCreateUser<TDto, TUser>(this IMappingExpression<TDto, TUser> mapper)
                where TUser : AbpUser<TUser>
        {
            return mapper.IncludeBase<UserCreateDto, AbpUser<TUser>>();
        }
        public static IMappingExpression<TUser,TDto> IncludeBaseSelectUser< TUser,TDto>(this IMappingExpression< TUser,TDto> mapper)
                where TUser : AbpUser<TUser>
        {
            return mapper.IncludeBase< AbpUser<TUser>,UserSelectDto>();
        }
        public static IMappingExpression<TUser, TDto> IncludeBaseUser< TUser,TDto>(this IMappingExpression<TUser, TDto> mapper)
                where TUser : AbpUser<TUser>
        {
            return mapper.IncludeBase<AbpUser<TUser>, UserDto>();
        }
    }
}