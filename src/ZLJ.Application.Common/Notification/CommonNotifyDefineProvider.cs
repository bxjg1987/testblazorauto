using Abp.Authorization;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZLJ.Application.Common.Notification
{
    /// <summary>
    /// 整个系统公用的通知定义，各app可以自己定义自己的
    /// </summary>
    public class CommonNotifyDefineProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            //var shebeiBaojing = new NotificationDefinition(Consts.ENEquipmentInstanceLastAlarmChanged,
            //                                               typeof(EquipmentInstanceEntity),
            //                                               displayName: Consts.ENEquipmentInstanceLastAlarmChanged.LICommon());
            //shebeiBaojing.Attributes.Add("Category", "设备状态变化");
            //shebeiBaojing.Attributes.Add("ComponentTypeName", "ZLJ.Notice.Simple");
            //context.Manager.Add(shebeiBaojing);

            //var shebeiZhuangtai = new NotificationDefinition(Consts.ENEquipmentInstanceLockStatusChanged,
            //                                               typeof(EquipmentInstanceEntity),
            //                                               displayName: Consts.ENEquipmentInstanceLockStatusChanged.LICommon());
            //shebeiZhuangtai.Attributes.Add("Category", "设备状态变化");
            //shebeiZhuangtai.Attributes.Add("ComponentTypeName", "ZLJ.Notice.Simple");

            //context.Manager.Add(shebeiZhuangtai);
        }
    }
}