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
using ZLJ.App.Admin.Sessions;

namespace ZLJ.App.Admin
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ZLJBaseAppService : CommonBaseApplicationService
    {
        public AdminSession AdminSession { get; set; }
        protected ZLJBaseAppService()
        {
            LocalizationSourceName = AdminConsts.Admin;
        }
    }
}
