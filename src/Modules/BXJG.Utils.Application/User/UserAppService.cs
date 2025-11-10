using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.User;
using BXJG.Utils.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.User
{
    /// <summary>
    /// 后台管理 用户 接口
    /// </summary>
    public class UserAppService<TUser, TRole, TUserManager,TRoleManager, TDto, TCondition, TCreateInput, TEditInput>
        : CrudBaseAppService<TUser, TDto, long,  PagedAndSortedResultRequest<TCondition>, TCreateInput, TEditInput>
          where TRole : AbpRole<TUser>, new()
           where TUser : AbpUser<TUser>//, IEntity<long>
        where TUserManager : IAbpUserManager<TRole, TUser>
        where TDto : IUserDto, IEntityDto<long>
        where TEditInput : IUserEditDto, IEntityDto<long> 
        where TCondition:class,new()
        where TRoleManager : IAbpRoleManager<TRole, TUser>
    {
        public TRoleManager roleManager { get; set; }
       public IPasswordHasher<TUser> passwordHasher { get; set; }
        public TUserManager userManager { get; set; }
        public UserAppService(IRepository<TUser, long> repository) : base(repository)
        {
        }

        protected  void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
