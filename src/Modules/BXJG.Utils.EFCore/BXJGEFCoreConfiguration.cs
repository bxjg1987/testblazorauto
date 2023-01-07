using Abp.Domain.Uow;
using BXJG.Utils.EFCore.CAP;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore
{
    public class BXJGEFCoreConfiguration
    {
        internal Func<IDispatcher, IActiveUnitOfWork, ICapTransaction> wt;

        /// <summary>
        /// 指定cap使用的dbcontext类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void InitDbContext<T>() where T : DbContext
        {
            wt = (c, uow) => new AbpCapTransaction<T>(c, uow);
        }
    }
}
