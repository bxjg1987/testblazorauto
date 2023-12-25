using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Authorization;
using ZLJ.Authorization;
using ZLJ.Localization;

namespace ZLJ.Navigation
{
    public partial class ZLJNavigationProvider
    {
        public void SetWMSNavigation(INavigationProviderContext context)
        {

            var wms = new MenuItemDefinition(PermissionNames.BXJGWMS,
                PermissionNames.BXJGWMS.GetLocalizableString(),
                icon: "cangku",
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMS));
            context.Manager.MainMenu.AddItem(wms);

            //仓库信息
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSHouse,
                displayName: PermissionNames.BXJGWMSHouse.GetLocalizableString(),
                icon: "cangku",
                url: $"/{PermissionNames.BXJGWMS}/house/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSHouse)));

            //仓库信息
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSBaseStore,
                displayName: PermissionNames.BXJGWMSBaseStore.GetLocalizableString(),
                icon: "cangku",
                url: $"/{PermissionNames.BXJGWMS}/basestore/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSBaseStore)));


            //入库管理
            wms.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGWMSStockIn,
                displayName: PermissionNames.BXJGWMSStockIn.GetLocalizableString(),
                icon: "cangku",
                url: $"/{PermissionNames.BXJGWMS}/stockin/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGWMSStockIn))
                );

        }
    }
}
