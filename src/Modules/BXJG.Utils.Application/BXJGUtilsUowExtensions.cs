using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    public static class BXJGUtilsUowExtensions
    {
        public static void DisableTenantFilter(this IActiveUnitOfWork uow)
        {
            uow.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant);
        }
    }
}
