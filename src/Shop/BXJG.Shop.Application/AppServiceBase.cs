using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
namespace BXJG.Shop
{
    /// <summary>
    /// 商城模块应用服务基类
    /// </summary>
    public abstract class AppServiceBase : ApplicationService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        protected AppServiceBase()
        {
            LocalizationSourceName = CoreConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
    }
}
