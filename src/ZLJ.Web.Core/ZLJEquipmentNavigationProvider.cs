using Abp.Application.Navigation;
using Abp.Authorization;
using ZLJ.Authorization;
using ZLJ.Localization;

namespace ZLJ.Navigation
{
    public partial class ZLJNavigationProvider
    {
        public void SetEquipmentNavigation(INavigationProviderContext context)
        {
            var equipment = new MenuItemDefinition(PermissionNames.BXJGEquipment,
                PermissionNames.BXJGEquipment.GetLocalizableString(),
                icon: "shuju",
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGEquipment));
            context.Manager.MainMenu.AddItem(equipment);

            //设备中心
            equipment.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGEquipmentInstance,
                displayName: PermissionNames.BXJGEquipmentInstance.GetLocalizableString(),
                icon: "gongju",
                url: $"/{PermissionNames.BXJGEquipment}/equipmentInstances/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGEquipmentInstance)));

            ////设备报表
            //equipment.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGEquipmentInstanceReport,
            //    displayName: PermissionNames.BXJGEquipmentInstanceReport.GetLocalizableString(),
            //    icon: "gongju",
            //    url: $"/{PermissionNames.BXJGEquipment}/BXJGEquipmentReport/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGEquipmentInstanceReport)));
        }
    }
}