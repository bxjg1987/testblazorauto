using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 物品模块应用服务基类
    /// 所有的物品模块应用服务都可以基础此类简化开发
    /// </summary>
    public abstract class AppServiceBase : ApplicationService
    {
        /// <summary>
        /// 物品模块内部的本地化源
        /// </summary>
        protected readonly Lazy<ILocalizationSource> BXJGGoodsInfoLocalizationSource;
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        protected AppServiceBase()
        {
            //构造函数中LocalizationManager应该是不可用的
            BXJGGoodsInfoLocalizationSource = new Lazy<ILocalizationSource>(() => LocalizationManager.GetSource(BXJGGoodsInfoCoreConsts.LocalizationSourceName));

            //LocalizationSourceName = CoreConsts.LocalizationSourceName;
        }


        protected virtual async Task CheckPermissionAsync(string permissionName)
        {
            //if (string.IsNullOrWhiteSpace(permissionName))
            //    return;

            //if (!await IsGrantedAsync(permissionName))
            //    throw new UserFriendlyException(L("UnAuthorized"));

            //使用父类的权限检查可以得到一个正常的未授权响应
            if (!string.IsNullOrEmpty(permissionName))
            {
                await PermissionChecker.AuthorizeAsync(permissionName);
            }
        }
        /// <summary>
        /// 从物品模块内部的源中获取本地化文本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string BXJGGoodsInfoL(string name)
        {
            return BXJGGoodsInfoLocalizationSource.Value.GetString(name);
        }
    }
}
