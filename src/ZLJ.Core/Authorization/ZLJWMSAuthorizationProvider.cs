using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using BXJG.Utils.Localization;
using ZLJ.Localization;

namespace ZLJ.Authorization
{
    public partial class ZLJAuthorizationProvider
    {
        public void SetWMSPermissions(Permission admin)
        {
            var wms = admin.CreateChildPermission(PermissionNames.BXJGWMS,
                PermissionNames.BXJGWMS.GetLocalizableString(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //仓库信息
            var house = wms.CreateChildPermission(PermissionNames.BXJGWMSHouse,
                PermissionNames.BXJGWMSHouse.GetLocalizableString(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            house.CreateChildPermission(PermissionNames.BXJGWMSHouseCreate,
                "新增".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            house.CreateChildPermission(PermissionNames.BXJGWMSHouseUpdate,
                "修改".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            house.CreateChildPermission(PermissionNames.BXJGWMSHouseDelete,
                "删除".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);


            ////整机档案
            //var machine = wms.CreateChildPermission(PermissionNames.BXJGWMSMachineArchives,
            //    PermissionNames.BXJGWMSMachineArchives.GetLocalizableString(),
            //    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            //machine.CreateChildPermission(PermissionNames.BXJGWMSMachineArchivesCreate,
            //    "新增".UtilsLI(),
            //    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            //machine.CreateChildPermission(PermissionNames.BXJGWMSMachineArchivesUpdate,
            //    "修改".UtilsLI(),
            //    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            //machine.CreateChildPermission(PermissionNames.BXJGWMSMachineArchivesDelete,
            //    "删除".UtilsLI(),
            //    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            //machine.CreateChildPermission(PermissionNames.BXJGWMSMachineFittingSetting,
            //    "配件".UtilsLI(),
            //    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);


            //仓库信息
            var basestore = wms.CreateChildPermission(PermissionNames.BXJGWMSBaseStore,
                PermissionNames.BXJGWMSBaseStore.GetLocalizableString(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            basestore.CreateChildPermission(PermissionNames.BXJGWMSBaseStoreCreate,
                "新增".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            basestore.CreateChildPermission(PermissionNames.BXJGWMSBaseStoreUpdate,
                "修改".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            basestore.CreateChildPermission(PermissionNames.BXJGWMSBaseStoreDelete,
                "删除".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //入库管理
            var stockin = wms.CreateChildPermission(PermissionNames.BXJGWMSStockIn,
                PermissionNames.BXJGWMSStockIn.GetLocalizableString(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            stockin.CreateChildPermission(PermissionNames.BXJGWMSStockInCreate,
                "新增".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            stockin.CreateChildPermission(PermissionNames.BXJGWMSStockInUpdate,
                "修改".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            stockin.CreateChildPermission(PermissionNames.BXJGWMSStockInDelete,
                "删除".UtilsLI(),
                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

        }
    }
}
