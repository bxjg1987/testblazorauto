using Abp.Authorization;
using BXJG.Equipment.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BXJG.Equipment.Authorization
{
    /// <summary>
    /// 设备管理模块针对模块内的本地化的扩展
    /// </summary>
    public static class BXJGEquipmentPermissionExt
    {
        /// <summary>
        /// 将设备管理模块中 权限树 的顶级节点 添加到 此节点的子节点中
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>设备管理模块中的根权限，即“设备管理”</returns>
        public static Permission AddEquipmentPermission(this Permission parent)
        {
            var root = parent.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipment,
                                                    BXJGEquipmentPermissionNames.BXJGEquipment.BXJGEquipmentL(),
                                                    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //这里是代码生成器会使用的占位符，代码生成器将在这里插入更多权限定义
            //{codegenerator}

            #region 设备档案
            var sbxx = root.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfo,
                                                  BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfo.BXJGEquipmentL(),
                                                  multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfoCreate,
                                       "新增".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfoUpdate,
                                       "修改".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfoDelete,
                                       "删除".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            #endregion

            return root;
        }
    }
}
