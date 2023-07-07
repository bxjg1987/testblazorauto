using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using ZLJ.App.Common;
using ZLJ.Web.Host.Startup;

namespace ZLJ.Controllers
{
    public abstract class ZLJControllerBase: AbpController
    {
        protected string CurrAppKey => base.HttpContext.GetAppKey();
        protected AppInfo CurrApp => base.HttpContext.GetApp();
        protected ZLJControllerBase()
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
