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
        ValueTask<long> GetCurrentCustomerIdAsync();
    }
}
