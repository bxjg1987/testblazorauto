using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using BXJG.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.Application.Common
{
    /// <summary>
    /// 非常curd类型的应用服务基类
    /// </summary>
    public abstract class CommonBaseAppService : ApplicationService
    {
        public TenantManager TenantManager { get; set; }
        //public IStaffSession StaffSession { get; set; }
        public UserManager UserManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        //public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        protected CommonBaseAppService()
        {
            LocalizationSourceName = Application.Common.Consts.Common;
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



        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != Application.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(Application.Common.Consts.Common);
                }

                return appCommonLocalizationSource;
            }
        }
        protected virtual ILocalizationSource LocalizationSourceAppZLJ
        {
            get
            {

                if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJ.Core.ZLJConsts.LocalizationSourceName)
                {
                    zljLocalizationSource = LocalizationManager.GetSource(ZLJ.Core.ZLJConsts.LocalizationSourceName);
                }

                return zljLocalizationSource;
            }
        }
        protected virtual ILocalizationSource LocalizationSourceUtils
        {
            get
            {

                if (utilsLocalizationSource == null || utilsLocalizationSource.Name != BXJGUtilsConsts.LocalizationSourceName)
                {
                    utilsLocalizationSource = LocalizationManager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
                }

                return utilsLocalizationSource;
            }
        }

    }
}
