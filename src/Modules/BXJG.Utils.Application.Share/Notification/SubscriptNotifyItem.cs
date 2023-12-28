using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Notification
{
    /// <summary>
    /// 批量订阅通知输入模型的项
    /// </summary>
    public class SubscriptNotifyItem
    {
        public string NotifyName { get; set; }
        public string EntityTypeName { get; set; }

        public string EntityId { get; set; }

        public override string ToString()
        {
            return $"{NotifyName} {EntityTypeName} {EntityId}";
        }
    }
}
