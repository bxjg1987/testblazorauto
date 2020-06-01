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
            var jczl = new MenuItemDefinition(BXJGCMSPermissions.BXJGCMS,
                                     BXJGCMSPermissions.BXJGCMS.BXJGCMSL(),
                                     icon: BXJGCMSPermissions.BXJGCMS,
                                     permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMS))
                .AddItem(new MenuItemDefinition(BXJGCMSPermissions.Article,
                                                BXJGCMSPermissions.Article.BXJGCMSL(),
                                                icon: BXJGCMSPermissions.Article,
                                                url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.Article}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.Article)))
                .AddItem(new MenuItemDefinition(BXJGCMSPermissions.Column,
                                                BXJGCMSPermissions.Column.BXJGCMSL(),
                                                icon: BXJGCMSPermissions.Column,
                                                url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.Column}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.Column)))
                .AddItem(new MenuItemDefinition(BXJGCMSPermissions.BXJGCMSAd,
                                                BXJGCMSPermissions.BXJGCMSAd.BXJGCMSL(),
                                                icon: BXJGCMSPermissions.BXJGCMSAd,
                                                url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.BXJGCMSAd}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMSAd)))
                .AddItem(new MenuItemDefinition(BXJGCMSPermissions.BXJGCMSAdPosition,
                                                BXJGCMSPermissions.BXJGCMSAdPosition.BXJGCMSL(),
                                                icon: BXJGCMSPermissions.BXJGCMSAdPosition,
                                                url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.BXJGCMSAdPosition}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMSAdPosition)))
                .AddItem(new MenuItemDefinition(BXJGCMSPermissions.BXJGCMSAdControl,
                                                BXJGCMSPermissions.BXJGCMSAdControl.BXJGCMSL(),
                                                icon: BXJGCMSPermissions.BXJGCMSAdControl,
                                                url: $"/{BXJGCMSPermissions.BXJGCMS}/{BXJGCMSPermissions.BXJGCMSAdControl}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGCMSPermissions.BXJGCMSAdControl)));

            menu.AddItem(jczl);
            return menu;
        }
    }
}
