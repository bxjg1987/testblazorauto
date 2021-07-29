using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.CMS.Authorization;
using BXJG.Equipment.Authorization;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.BaseInfo.Authorization;
using BXJG.WorkOrder;

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
            #region 设备
            admin.AddEquipmentPermission();
            #endregion
            #region 工单
            //admin.AddBXJGWorkOrderAllPermission();
            #endregion
            #region 商城
            admin.AddBXJGShopPermission();
            #endregion
            #region CMS
            BXJGCMSAuthorizationProvider.SetPermissions(admin);
            #endregion
            #region 加盟商
            //var franchisee = admin.CreateChildPermission(PermissionNames.Franchisee, L("Franchisee"));
            //franchisee.CreateChildPermission(PermissionNames.FranchiseeInfo, L("FranchiseeInfo"));
            //franchisee.CreateChildPermission(PermissionNames.FranchiseeEquipment, L("FranchiseeEquipment"));



            //var franchiseeBack = admin.CreateChildPermission(PermissionNames.FranchiseeBack, L("FranchiseeBack"));

            //var franchiseeBackStatistical = franchiseeBack.CreateChildPermission(PermissionNames.FranchiseeBackStatistical, L("FranchiseeBackStatistical"));
            //franchiseeBackStatistical.CreateChildPermission(PermissionNames.FranchiseeBackStatisticalIncome, L("FranchiseeBackStatisticalIncome"));
            //franchiseeBackStatistical.CreateChildPermission(PermissionNames.FranchiseeBackStatisticalUser, L("FranchiseeBackStatisticalUser"));
            //franchiseeBackStatistical.CreateChildPermission(PermissionNames.FranchiseeBackStatisticalOrder, L("FranchiseeBackStatisticalOrder"));
            //franchiseeBackStatistical.CreateChildPermission(PermissionNames.FranchiseeBackStatisticalSale, L("FranchiseeBackStatisticalSale"));

            //var franchiseeBackEquipment = franchiseeBack.CreateChildPermission(PermissionNames.FranchiseeBackEquipment, L("FranchiseeBackEquipment"));
            //franchiseeBackEquipment.CreateChildPermission(PermissionNames.FranchiseeBackEquipmentOrderStatus, L("FranchiseeBackEquipmentOrderStatus"));
            //franchiseeBackEquipment.CreateChildPermission(PermissionNames.FranchiseeBackEquipmentStatus, L("FranchiseeBackEquipmentStatus"));
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
