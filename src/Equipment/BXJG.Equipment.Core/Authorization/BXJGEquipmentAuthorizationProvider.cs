using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Equipment.Localization;

namespace BXJG.Equipment.Authorization
{
    public class BXJGEquipmentAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var root = context.CreatePermission(BXJGEquipmentPermissionNames.BXJGEquipment,
                                                 BXJGEquipmentPermissionNames.BXJGEquipment.BXJGEquipmentL());
            //{codegenerator}

            //#region 商城
            //BXJGEquipmentAuthorizationProvider.SetPermissions(admin);
            //#endregion
            //#region CMS
            //BXJGCMSAuthorizationProvider.SetPermissions(admin);
            //#endregion
            //#region 资产管理
            //var asset = admin.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorAsset, L("Asset management"), multiTenancySides: MultiTenancySides.Tenant);

            //var equipmentInfo = asset.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorAssetEquipmentInfo, L("Equipment info"), multiTenancySides: MultiTenancySides.Tenant);
            //asset.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorAssetEquipmentInfoCreate, L("Create"), multiTenancySides: MultiTenancySides.Tenant);
            //asset.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorAssetEquipmentInfoUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //asset.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorAssetEquipmentInfoDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //#endregion
            //#region 基础资料
            //var baseInfo = admin.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfo, L("BaseInfo"), multiTenancySides: MultiTenancySides.Tenant);

            //var btype = baseInfo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoBtype, L("Btype"), multiTenancySides: MultiTenancySides.Tenant);
            //btype.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoBtypeCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //btype.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoBtypeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //btype.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoBtypeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //var uo = baseInfo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoOrganizationUnit, L("OrganizationUnit"), multiTenancySides: MultiTenancySides.Tenant);
            //uo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoOrganizationUnitAdd, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //uo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoOrganizationUnitUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //uo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoOrganizationUnitDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //var job = baseInfo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoJob, L("Job"), multiTenancySides: MultiTenancySides.Tenant);
            //job.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoJobCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //job.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoJobUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //job.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoJobDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //var emp = baseInfo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoEmployee, L("Employee"), multiTenancySides: MultiTenancySides.Tenant);
            //emp.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoEmployeeCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //emp.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoEmployeeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //emp.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoEmployeeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //var xz = baseInfo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoAdministrative, L("Administrative"), multiTenancySides: MultiTenancySides.Tenant);
            //xz.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoAdministrativeAdd, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            //xz.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoAdministrativeUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            //xz.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorBaseInfoAdministrativeDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);

            //cfg.InitPermission(baseInfo);
            ////var zd = baseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionary, L("DataDictionary"), multiTenancySides: MultiTenancySides.Tenant);
            ////zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryCreate, L("Add"), multiTenancySides: MultiTenancySides.Tenant);
            ////zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryUpdate, L("Update"), multiTenancySides: MultiTenancySides.Tenant);
            ////zd.CreateChildPermission(PermissionNames.AdministratorBaseInfoDataDictionaryDelete, L("Delete"), multiTenancySides: MultiTenancySides.Tenant);


            //#endregion
            //#region 系统管理
            //var sys = admin.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystem, L("System"));
            //sys.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemTenant, L("Tenant"), multiTenancySides: MultiTenancySides.Host);

            //var roleM = sys.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemRole, L("Role"));
            //roleM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemRoleAdd, L("Add"));
            //roleM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemRoleUpdate, L("Update"));
            //roleM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemRoleDelete, L("Delete"));

            //var userM = sys.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemUser, L("User"));
            //userM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemUserAdd, L("Add"));
            //userM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemUserUpdate, L("Update"));
            //userM.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemUserDelete, L("Delete"));

            //sys.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemLog, L("Log"));
            //sys.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorSystemConfig, L("Settings"));
            //#endregion
            //#region Demo
            //var demo = admin.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorDemo, L("Demo"), multiTenancySides: MultiTenancySides.Tenant);
            //demo.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorDemoUpload, L("Upload"), multiTenancySides: MultiTenancySides.Tenant);
            //#endregion
            //#region 微信
            //var weChat = admin.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorWeChat, L("WeChat"), multiTenancySides: MultiTenancySides.Tenant);
            //weChat.CreateChildPermission(BXJGEquipmentPermissionNames.AdministratorWeChatIndex, L("WeChatIndex"), multiTenancySides: MultiTenancySides.Tenant);
            //#endregion 
        }

        //private static ILocalizableString L(string name)
        //{
        //    return new LocalizableString(name, BXJGEquipmentConst.LocalizationSourceName);
        //}
    }
}
