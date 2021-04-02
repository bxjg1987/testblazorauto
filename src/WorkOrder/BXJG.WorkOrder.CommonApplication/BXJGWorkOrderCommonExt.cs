using Abp.Authorization.Users;
using Abp.Dependency;
using BXJG.WorkOrder.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder
{
    public static class BXJGWorkOrderCommonExt
    {
        public static IIocManager RegisterBXJGWorkOrderDefaultAdapter<TUser>(this IIocManager iocManager) where TUser : AbpUser<TUser>
        {
            //模块调用方可以替换此服务
            iocManager.Register(typeof(IEmployeeAppService), typeof(EmployeeAppService<TUser>), DependencyLifeStyle.Transient);
            return iocManager;
        }
    }
}
