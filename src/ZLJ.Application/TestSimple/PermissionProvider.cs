using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;
using ZLJ.Core.Localization;
using System.Collections.Generic;
using ZLJ.Application.Share.TestSimple;

namespace ZLJ.Application.Authorization.Permissions
{
    public partial class AdminAuthorizationProvider 
    {
        public void CodeGeneratorTestSimple(IPermissionDefinitionContext context)
        {

            var parent = context.GetPermissionOrNull("Administrator");
            var permission = parent.CreateChildPermission(TestSimpleApplicationShareConsts.PermissionNameGet,
                                                          L(TestSimpleApplicationShareConsts.PermissionNameGet),
                                                          multiTenancySides: MultiTenancySides.Tenant, 
                                                          properties: new Dictionary<string, object> { { "icon", "test" } });

            permission.CreateChildPermission(TestSimpleApplicationShareConsts.PermissionNameCreate,
                                             "新增".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "plush" } });
            permission.CreateChildPermission(TestSimpleApplicationShareConsts.PermissionNameUpdate,
                                             "修改".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "add" } });
            permission.CreateChildPermission(TestSimpleApplicationShareConsts.PermissionNameDelete,
                                             "删除".UtilsLI(),
                                             multiTenancySides: MultiTenancySides.Tenant, 
                                             properties: new Dictionary<string, object> { { "btn", true }, { "icon", "clear" } });
        }
    }
}