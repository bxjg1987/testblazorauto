using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;

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
            #region 商城
            BXJGShopAuthorizationProvider.SetPermissions(admin);
            #endregion
            #region 资产管理
            var asset = admin.CreateChildPermission(PermissionNames.AdministratorAsset, L("Asset management"), multiTenancySides: MultiTenancySides.Tenant);

            var equipmentInfo = asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfo, L("Equipment info"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoCreate, L("Create"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            asset.CreateChildPermission(PermissionNames.AdministratorAssetEquipmentInfoDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            #endregion
            #region 基础资料
            var baseInfo = admin.CreateChildPermission(PermissionNames.AdministratorBaseInfo, L("BaseInfo"), multiTenancySides: MultiTenancySides.Tenant);

            var btype = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoBtype, L("Btype"), multiTenancySides: MultiTenancySides.Tenant);
            btype.CreateChildPermission(PermissionNames.AdministratorBaseInfoBtypeCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            btype.CreateChildPermission(PermissionNames.AdministratorBaseInfoBtypeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            btype.CreateChildPermission(PermissionNames.AdministratorBaseInfoBtypeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            var uo = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnit, L("OrganizationUnit"), multiTenancySides: MultiTenancySides.Tenant);
            uo.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitAdd, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            uo.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            uo.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            var job = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoJob, L("Job"), multiTenancySides: MultiTenancySides.Tenant);
            job.CreateChildPermission(PermissionNames.AdministratorBaseInfoJobCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            job.CreateChildPermission(PermissionNames.AdministratorBaseInfoJobUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            job.CreateChildPermission(PermissionNames.AdministratorBaseInfoJobDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            var emp = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoEmployee, L("Employee"), multiTenancySides: MultiTenancySides.Tenant);
            emp.CreateChildPermission(PermissionNames.AdministratorBaseInfoEmployeeCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            emp.CreateChildPermission(PermissionNames.AdministratorBaseInfoEmployeeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            emp.CreateChildPermission(PermissionNames.AdministratorBaseInfoEmployeeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            var xz = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoAdministrative, L("Administrative"), multiTenancySides: MultiTenancySides.Tenant);
            xz.CreateChildPermission(PermissionNames.AdministratorBaseInfoAdministrativeAdd, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            xz.CreateChildPermission(PermissionNames.AdministratorBaseInfoAdministrativeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            xz.CreateChildPermission(PermissionNames.AdministratorBaseInfoAdministrativeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            cfg.InitPermission( baseInfo);
            //var zd = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionary, L("DataDictionary"), multiTenancySides: MultiTenancySides.Tenant);
            //zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);


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
            #region Demo
            var demo = admin.CreateChildPermission(PermissionNames.AdministratorDemo, L("Demo"), multiTenancySides: MultiTenancySides.Tenant);
            demo.CreateChildPermission(PermissionNames.AdministratorDemoUpload, L("Upload"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion
            #region 微信
            var weChat = admin.CreateChildPermission(PermissionNames.AdministratorWeChat, L("WeChat"), multiTenancySides: MultiTenancySides.Tenant);
            weChat.CreateChildPermission(PermissionNames.AdministratorWeChatIndex, L("WeChatIndex"), multiTenancySides: MultiTenancySides.Tenant);
            #endregion 
        }

            private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}
