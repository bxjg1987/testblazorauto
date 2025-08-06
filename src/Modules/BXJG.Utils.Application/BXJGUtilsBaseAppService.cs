using Abp.Application.Services;
using Abp.Linq;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.Utils.Share;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace BXJG.Utils.Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class BXJGUtilsBaseAppService : ApplicationService
    {
        //public TenantManager TenantManager { get; set; }

        public IHostEnvironment HostEnvironment { get; set; }
        //public UserManager UserManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        public ICancellationTokenProvider CancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance;
        protected BXJGUtilsBaseAppService()
        {
            LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
        }

        //protected virtual async Task<User> GetCurrentUserAsync()
        //{
        //    var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        //    if (user == null)
        //    {
        //        throw new Exception("There is no current user!");
        //    }

        //    return user;
        //}

        //protected virtual Task<Tenant> GetCurrentTenantAsync()
        //{
        //    return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        //}

        //protected virtual void CheckErrors(IdentityResult identityResult)
        //{
        //    identityResult.CheckErrors(LocalizationManager);
        //}
    }
}
