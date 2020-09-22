using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.CMS.Authorization;
using BXJG.Equipment.Authorization;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.BaseInfo.Authorization;
namespace ZLJ.Authorization
{
    public class ZLJAuthorizationProvider : AuthorizationProvider
    {
        GeneralTreeModuleConfig cfg;
        public ZLJAuthorizationProvider(GeneralTreeModuleConfig cfg)
        {
            this.cfg = cfg;
        }
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var admin = context.CreatePermission(PermissionNames.Administrator, L("Administrator"));
            //{codegenerator}

            //添加设备管理模块的权限
            admin.AddEquipmentPermission();

            #region 商城
            admin.AddBXJGShopPermission();
            #endregion
            #region CMS
            BXJGCMSAuthorizationProvider.SetPermissions(admin);
            #endregion
            #region 资产管理
            var asset = admin.CreateChildPermission(PermissionNames.AdministratorAsset, L("Asset management"), multiTenancySides: MultiTenancySides.Tenant);

            var equipmentInfo = asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfo, L("Equipment info"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoCreate, L("Create"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion
            #region 基础信息
             admin.AddBaseInfoPermission();
            #endregion
            #region 系统管理
            var sys = admin.CreateChildPermission(PermissionNames.AdministratorSystem, L("System"));
            sys.CreateChildPermission(PermissionNames.AdministratorSystemTenant, L("Tenant"), multiTenancySides: MultiTenancySides.Host);

            var roleM = sys.CreateChildPermission(PermissionNames.AdministratorSystemRole, L("Role"));
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleAdd, L("Add"));
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleUpdate, L("Update"));
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleDelete, L("Delete"));

            var userM = sys.CreateChildPermission(PermissionNames.AdministratorSystemUser, L("User"));
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserAdd, L("Add"));
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserUpdate, L("Update"));
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserDelete, L("Delete"));

            sys.CreateChildPermission(PermissionNames.AdministratorSystemLog, L("Log"));
            sys.CreateChildPermission(PermissionNames.AdministratorSystemConfig, L("Settings"));
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}
