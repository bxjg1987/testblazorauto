using Abp;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Abp
{
    public static class BXJGUtilsAbpExtentions
    {
        /// <summary>
        /// 禁用数据过滤器
        /// <see href="https://aspnetboilerplate.com/Pages/Documents/Data-Filters#about-the-using-statement"/>
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="filterNames"></param>
        public static void DisableFilter(this AbpServiceBase appService, params string[] filterNames)
        {
            appService.UnitOfWorkManager.Current.DisableFilter(filterNames);
        }
        /// <summary>
        /// 禁用租户过滤器
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="filterNames"></param>
        public static void DisableTenantFilter(this AbpServiceBase appService, params string[] filterNames)
        {
            appService.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant);
        }
    }
}
