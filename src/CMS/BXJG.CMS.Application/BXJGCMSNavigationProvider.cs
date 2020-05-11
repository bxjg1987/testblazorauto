using Abp.Application.Navigation;
using Abp.Authorization;
using BXJG.CMS.Authorization;
using BXJG.CMS.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS
{
    public static class BXJGCMSNavigationProvider
    {
        public static MenuDefinition Init(MenuDefinition menu)
        {
            //var jczl = new MenuItemDefinition(BXJGCMSPermissions.BXJGCMS,
            //                         BXJGCMSPermissions.BXJGCMS.BXJGCMSL(),
            //                         icon: BXJGCMSPermissions.BXJGCMS,
            //                         permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMS))
            //    .AddItem(new MenuItemDefinition(BXJGCMSPermissions.BXJGCMSDictionary,
            //                                    BXJGCMSPermissions.BXJGCMSDictionary.BXJGCMSL(),
            //                                    icon: BXJGCMSPermissions.BXJGCMSDictionary,
            //                                    url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.BXJGCMSDictionary}/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMSDictionary)))
            //    .AddItem(new MenuItemDefinition(BXJGCMSPermissions.BXJGCMSItem,
            //                                    BXJGCMSPermissions.BXJGCMSItem.BXJGCMSL(),
            //                                    icon: BXJGCMSPermissions.BXJGCMSItem,
            //                                    url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.BXJGCMSItem}/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMSItem)));

            //menu.AddItem(jczl);
            return menu;
        }
    }
}
