using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ZLJ.Core.MultiTenancy;
using Abp.Linq;
using ZLJ.Core.Authorization.Users;
using Abp.Localization.Sources;
using BXJG.Utils;
using ZLJ.Application.Share;

namespace ZLJ.Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class AdminBaseAppService : CommonBaseAppService
    {
        protected AdminBaseAppService()
        {
            LocalizationSourceName = AdminConsts.Admin;
        }
    }
}
