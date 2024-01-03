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
using BXJG.Utils;

namespace ZLJ.App.Admin
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
