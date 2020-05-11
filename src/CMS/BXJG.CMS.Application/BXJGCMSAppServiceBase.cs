using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
namespace BXJG.CMS
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class BXJGCMSAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager> : ApplicationService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>,new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager: AbpTenantManager<TTenant, TUser>
        where TUserManager: AbpUserManager<TRole, TUser>
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        public TTenantManager TenantManager { get; set; }

        public TUserManager UserManager { get; set; }
        //参考BXJGCMSDomainServiceBase中的注释
        //public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;
        protected BXJGCMSAppServiceBase()
        {
            LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        protected virtual async Task<TUser> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<TTenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
