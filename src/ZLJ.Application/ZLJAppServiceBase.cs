using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ZLJ.MultiTenancy;
using Abp.Linq;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using ZLJ.Authorization.Users;
using Abp.Localization.Sources;

namespace ZLJ.App.Admin
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ZLJAppServiceBase : ApplicationService
    {
        //public new IAbpSession AbpSession { 
        
        //}
        public TenantManager TenantManager { get; set; }
        //public IStaffSession StaffSession { get; set; }
        public UserManager UserManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        protected ZLJAppServiceBase()
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
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


        //
        // 摘要:
        //     Gets/sets name of the localization source that is used in this application service.
        //     It must be set in order to use Abp.AbpServiceBase.L(System.String) and Abp.AbpServiceBase.L(System.String,System.Globalization.CultureInfo)
        //     methods.
       // protected string LocalizationSourceNameAdmin { get; set; } = AdminConsts.Admin;
        private ILocalizationSource _localizationSourceAdmin;
        //
        // 摘要:
        //     Gets localization source. It's valid if Abp.AbpServiceBase.LocalizationSourceName
        //     is set.
        protected ILocalizationSource LocalizationSourceAdmin
        {
            get
            {

                if (_localizationSourceAdmin == null || _localizationSourceAdmin.Name != AdminConsts.Admin)
                {
                    _localizationSourceAdmin = LocalizationManager.GetSource(AdminConsts.Admin);
                }

                return _localizationSourceAdmin;
            }
        }
    }

    ///// <summary>
    ///// 员工端应用服务基类  已经移植到ZLJ.Application.Employee
    ///// </summary>
    //public class StaffAppServiceBase : ZLJAppServiceBase
    //{
    //    private readonly IStaffSession staffSession;

    //    public StaffAppServiceBase(IStaffSession staffSession)
    //    {
    //        this.staffSession = staffSession;
    //    }


    //}
}
