using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.GeneralTree;
//using BXJG.WorkOrder;
using BXJG.Utils.Localization;
using ZLJ.Localization;
using System.Collections.Generic;

namespace ZLJ.Authorization
{
    public partial class ZLJAuthorizationProvider : AuthorizationProvider
    {
        //GeneralTreeModuleConfig cfg;

        //public ZLJAuthorizationProvider(GeneralTreeModuleConfig cfg)
        //{
        //    this.cfg = cfg;
        //}

        public override void SetPermissions(IPermissionDefinitionContext context)
        {

            #region 被依赖的权限
            //context.CreatePermission(PermissionNames.CommonGetRentOrderDetail);
            #endregion

            #region 后端
            var admin = context.CreatePermission(PermissionNames.Administrator, L("Administrator"));


            

            

            #endregion


            #region 基础信息

            var permissionBaseInfo = admin.CreateChildPermission(PermissionNames.AdministratorBaseInfo,
                PermissionNames.AdministratorBaseInfo.GetLocalizableString(),
                multiTenancySides: MultiTenancySides.Tenant);

            #region 公司和部门
            var ou = permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnit,
               PermissionNames.AdministratorBaseInfoOrganizationUnit.GetLocalizableString(),
               multiTenancySides: MultiTenancySides.Tenant);
            ou.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitAdd,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            ou.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitUpdate,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            ou.CreateChildPermission(PermissionNames.AdministratorBaseInfoOrganizationUnitDelete,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            #endregion


            #region 岗位
            var post = permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorBaseInfoPost,
               PermissionNames.AdministratorBaseInfoPost.GetLocalizableString(),
               multiTenancySides: MultiTenancySides.Tenant);
            post.CreateChildPermission(PermissionNames.AdministratorBaseInfoPostCreate,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            post.CreateChildPermission(PermissionNames.AdministratorBaseInfoPostUpdate,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            post.CreateChildPermission(PermissionNames.AdministratorBaseInfoPostDelete,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            #endregion

            #region 地区

            var sbxx = permissionBaseInfo.CreateChildPermission(PermissionNames.BXJGBaseInfoAdministrative,
                PermissionNames.BXJGBaseInfoAdministrative.GetLocalizableString(),
                multiTenancySides: MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(PermissionNames.BXJGBaseInfoAdministrativeCreate,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            sbxx.CreateChildPermission(PermissionNames.BXJGBaseInfoAdministrativeUpdate,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            sbxx.CreateChildPermission(PermissionNames.BXJGBaseInfoAdministrativeDelete,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });

            #endregion

            #region 员工档案

            var staffInfo = permissionBaseInfo.CreateChildPermission(PermissionNames.BXJGBaseInfoStaffInfo,
                PermissionNames.BXJGBaseInfoStaffInfo.GetLocalizableString(),
                multiTenancySides: MultiTenancySides.Tenant);
            staffInfo.CreateChildPermission(PermissionNames.BXJGBaseInfoStaffInfoCreate,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            staffInfo.CreateChildPermission(PermissionNames.BXJGBaseInfoStaffInfoUpdate,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            staffInfo.CreateChildPermission(PermissionNames.BXJGBaseInfoStaffInfoDelete,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });

            #endregion

            # region 来往单位

            var associatedCompany = permissionBaseInfo.CreateChildPermission(
                PermissionNames.BXJGBaseInfoAssociatedCompany,
                PermissionNames.BXJGBaseInfoAssociatedCompany.GetLocalizableString(),
                multiTenancySides: MultiTenancySides.Tenant);
            associatedCompany.CreateChildPermission(PermissionNames.BXJGBaseInfoAssociatedCompanyCreate,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            associatedCompany.CreateChildPermission(PermissionNames.BXJGBaseInfoAssociatedCompanyUpdate,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            associatedCompany.CreateChildPermission(PermissionNames.BXJGBaseInfoAssociatedCompanyDelete,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });

            #endregion



            //耗材档案
            //var consume = permissionBaseInfo.CreateChildPermission(PermissionNames.BXJGWMSConsumeArchives,
            //    PermissionNames.BXJGWMSConsumeArchives.GetLocalizableString(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //consume.CreateChildPermission(PermissionNames.BXJGWMSConsumeArchivesCreate,
            //    "新增".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //consume.CreateChildPermission(PermissionNames.BXJGWMSConsumeArchivesUpdate,
            //    "修改".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //consume.CreateChildPermission(PermissionNames.BXJGWMSConsumeArchivesDelete,
            //    "删除".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //consume.CreateChildPermission(PermissionNames.BXJGWMSSFittingMachinePatternSetting,
            //    "整机型号".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //配件档案
            //var fitting = permissionBaseInfo.CreateChildPermission(PermissionNames.BXJGWMSFittingArchives,
            //    PermissionNames.BXJGWMSFittingArchives.GetLocalizableString(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //fitting.CreateChildPermission(PermissionNames.BXJGWMSFittingArchivesCreate,
            //    "新增".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //fitting.CreateChildPermission(PermissionNames.BXJGWMSFittingArchivesUpdate,
            //    "修改".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);
            //fitting.CreateChildPermission(PermissionNames.BXJGWMSFittingArchivesDelete,
            //    "删除".UtilsLI(),
            //    multiTenancySides: MultiTenancySides.Tenant);

            permissionBaseInfo.AddGeneralTreePermission();

            //--codegenerator.BaseInfo==

            //#endregion

            //#region 系统管理

            //var sys = admin.CreateChildPermission(PermissionNames.AdministratorSystem, L("System"));
            permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorSystemTenant, L("Tenant"),
                multiTenancySides: MultiTenancySides.Host);

            var roleM = permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorSystemRole, L("Role"));
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleAdd, L("Add"), properties: new Dictionary<string, object> { { "btn", true } });
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleUpdate, L("Update"), properties: new Dictionary<string, object> { { "btn", true } });
            roleM.CreateChildPermission(PermissionNames.AdministratorSystemRoleDelete, L("Delete"), properties: new Dictionary<string, object> { { "btn", true } });

            var userM = permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorSystemUser, L("User"));
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserAdd, L("Add"), properties: new Dictionary<string, object> { { "btn", true } });
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserUpdate, L("Update"), properties: new Dictionary<string, object> { { "btn", true } });
            userM.CreateChildPermission(PermissionNames.AdministratorSystemUserDelete, L("Delete"), properties: new Dictionary<string, object> { { "btn", true } });

            permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorSystemLog, L("Log"));
            permissionBaseInfo.CreateChildPermission(PermissionNames.AdministratorSystemConfig, L("Settings"));

            permissionBaseInfo.CreateChildPermission(PermissionNames.HangFireDashboard, L("HangFireDashboard"));

            //sys.CreateChildPermission(PermissionNames.AdministratorSystemSetting, L("Settings"));
            #endregion


            //#endregion

            


 //           var cust = context.CreatePermission(PermissionNames.Customer,
 //PermissionNames.Customer.GetLocalizableString(),
 //multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);


        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}