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
        [Obsolete("abp为了使用ef的强大功能，仓储实现得并不纯粹，所以我们决定直接引入ef的包，具体参考：https://gitee.com/bxjg1987/abp/wikis/%E5%85%B3%E4%BA%8E%E6%95%B0%E6%8D%AE%E8%AE%BF%E9%97%AE?sort_id=3479400")]
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        protected AppServiceBase()
        {
            LocalizationSourceName = CoreConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
    }
}
