using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客session，用来获取当前登陆的顾客id
    /// </summary>
    public interface ICustomerSession
    {
        /// <summary>
        /// 获取当前登陆用户关联的顾客信息
        /// 若当前登陆用户不属于Customer角色则返回null
        /// </summary>
        /// <returns></returns>
        ValueTask<long?> GetCurrentCustomerIdAsync();
    }
}
