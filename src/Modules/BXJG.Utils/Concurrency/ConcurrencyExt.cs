using Abp.Domain.Uow;
using BXJG.Utils.Share.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Concurrency
{
    public static class ConcurrencyExt
    {
        //public static void CheckConcurrency(this IHasConcurrencyStamp stamp, string str)
        //{
        //    if (stamp.ConcurrencyStamp != str)
        //        throw new AbpDbConcurrencyException();
        //}

        //放心用，ef的乐观并发并不会取实体的token进行比较，而是与数据库种的原始值进行比较的
        //我们这里比较一来是为了更早的触发并发冲突，再者是给实体并发标记赋新值

        public static void CheckAndUpdateConcurrency(this IHasConcurrencyStamp stamp, string str)
        {
            if (stamp.ConcurrencyStamp != str)
                throw new AbpDbConcurrencyException();

            stamp.ConcurrencyStamp = Guid.NewGuid().ToString("n");
        }
        public static void CheckAndUpdateConcurrency(this IHasConcurrencyStamp stamp, IHasConcurrencyStamp stamp2)
        {
            if (stamp.ConcurrencyStamp != stamp2.ConcurrencyStamp)
                throw new AbpDbConcurrencyException();

            stamp.ConcurrencyStamp = Guid.NewGuid().ToString("n");
            stamp2.ConcurrencyStamp = stamp.ConcurrencyStamp;
        }
    }
}
