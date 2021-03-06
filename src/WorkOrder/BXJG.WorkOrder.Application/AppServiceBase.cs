using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
namespace BXJG.WorkOrder
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
        }
    }
}
