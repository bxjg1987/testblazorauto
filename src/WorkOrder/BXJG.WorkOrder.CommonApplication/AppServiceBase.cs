using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
namespace BXJG.WorkOrder
{
    /// <summary>
    /// 工单模块应用服务基类
    /// </summary>
    public abstract class AppServiceBase : ApplicationService
    {
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        protected AppServiceBase()
        {
            LocalizationSourceName = CoreConsts.LocalizationSourceName;
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
    }
}
