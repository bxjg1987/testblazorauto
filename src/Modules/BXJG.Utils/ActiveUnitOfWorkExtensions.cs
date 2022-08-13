using Abp.Domain.Uow;
using Abp.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{

    /*
     * 比如，一个合同只允许存在一个未审核的合同变更单
     * 线程1在审核，锁完成，线程2新增，成功，此时1事务回滚会导致出现两天未审核的合同变更单
     * 因此这里要求锁的范围大于事务的范围，事务完成时才释放锁
     * 
     * 遗憾的是abp的dispose事件目前没有异步版本，已经在github提交issue
     */

    public static class ActiveUnitOfWorkExtensions
    {
        public const string __disposeableObject = "__disposeableObject";
        public static void AddDisposeableObject(this IActiveUnitOfWork uow, object obj)
        {
            if (uow.Items.TryGetValue(__disposeableObject, out var t))
            {
                (t as HashSet<object>).Add(obj);
            }
            else
            {
                var s = new HashSet<object>();
                s.Add(obj);
                uow.Items[__disposeableObject] = s;
            }
            // if(EventHandler.)

            //uow.Disposed -= Uow_Disposed;
            //uow.Disposed += Uow_Disposed;
        }

        
    }
}
