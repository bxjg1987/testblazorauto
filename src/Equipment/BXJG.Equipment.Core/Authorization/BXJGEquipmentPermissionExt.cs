using Abp.Authorization;
using BXJG.Equipment.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BXJG.Equipment.Authorization
{
    public static class BXJGEquipmentPermissionExt
    {
        /// <summary>
        /// 将设备管理模块中 权限树 的顶级节点 添加到 此节点的子节点中。返回此节点以便实现链式编程
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Permission AddEquipmentPermission(this Permission parent)
        {
            var root = parent.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipment,
                                                    BXJGEquipmentPermissionNames.BXJGEquipment.BXJGEquipmentL(),
                                                    "设备管理菜单描述信息".BXJGEquipmentL(),
                                                    Abp.MultiTenancy.MultiTenancySides.Tenant);

            //添加当前模块的更多子权限

            return parent;
        }
    }
}
