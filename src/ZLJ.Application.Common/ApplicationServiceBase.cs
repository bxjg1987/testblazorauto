using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Common
{
    public class ApplicationServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        //public IStaffSession StaffSession { get; set; }
        public UserManager UserManager { get; set; }
        //public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        protected ApplicationServiceBase()
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            //  AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
