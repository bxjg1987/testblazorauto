using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ZLJ.Controllers
{
    public abstract class ZLJControllerBase: AbpController
    {
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
