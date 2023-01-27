using Abp.Domain.Uow;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils;
using BXJG.Utils.EFCore.CAP;

namespace BXJG.Utils
{
    public static class BXJGEFCoreConfiguration1
    {
      //  internal Func<IDispatcher, IActiveUnitOfWork, ICapTransaction> wt;

        /// <summary>
        /// 指定cap使用的dbcontext类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void InitDbContext<T>(this BXJGUtilsModuleConfig cfg) where T : DbContext
        {
           // cfg.wt = (c, uow) => new AbpCapTransaction<T>(c, uow);
        }
    }
}
