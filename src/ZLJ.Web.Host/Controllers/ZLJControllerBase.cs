using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using ZLJ.Application.Common;
using ZLJ.Web.Host.Startup;

namespace ZLJ.Controllers
{
    public abstract class ZLJControllerBase: AbpController
    {
        protected ZLJControllerBase()
        {
            LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
