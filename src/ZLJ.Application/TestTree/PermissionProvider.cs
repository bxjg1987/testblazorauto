using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;
using ZLJ.Core.Localization;
using System.Collections.Generic;
using ZLJ.Application.Share.TestTree;

namespace ZLJ.Application.Authorization.Permissions
{
    public partial class AdminAuthorizationProvider 
    {
        public void CodeGeneratorTestTree(IPermissionDefinitionContext context)
        {
            var parent = context.GetPermissionOrNull("Administrator");
            var permission = parent.CreateChildPermission(TestTreeApplicationShareConsts.PermissionNameGet,
                                                          L(TestTreeApplicationShareConsts.PermissionNameGet),
                                                          multiTenancySides: MultiTenancySides.Tenant, 
                                                          properties: new Dictionary<string, object> { { "icon", "testtree" } });

            permission.CreateChildPermission(TestTreeApplicationShareConsts.PermissionNameCreate,
                                             "新增".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "plush" } });
            permission.CreateChildPermission(TestTreeApplicationShareConsts.PermissionNameUpdate,
                                             "修改".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "add" } });
            permission.CreateChildPermission(TestTreeApplicationShareConsts.PermissionNameDelete,
                                             "删除".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "clear" } });
        }
    }
}