using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Share.DataPermission
{
    /// <summary>
    /// 用于启用数据权限拦截，可应用于类或方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DataPermissionAttribute : Attribute
    {
    }
    /// <summary>
    /// 用于在方法级别禁用数据权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DisableDataPermissionAttribute : Attribute
    {
    }
}
