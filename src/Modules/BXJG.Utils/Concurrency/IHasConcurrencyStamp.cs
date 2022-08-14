using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Concurrency
{
    public interface IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
    //public class ConcurrencyException : Exception { 

    //}
    //这个扩展可以考虑 automapper也调用下
    public static class ConcurrencyExt
    {
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
