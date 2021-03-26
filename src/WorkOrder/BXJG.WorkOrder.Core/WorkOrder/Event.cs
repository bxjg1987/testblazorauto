using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class StatusChangedEventData : EntityEventData<OrderBaseEntity>
    {
        public Status Original { get; private set; }
        public StatusChangedEventData(OrderBaseEntity entity, Status original) : base(entity)
        {
            Original = original;
        }
    }
    ////通知冗余字段引用方修改自己的数据
    public class EstimatedTimeChangedEventData : EntityEventData<OrderBaseEntity>
    {
        public DateTimeOffset? OriginalStart { get; private set; }
        public DateTimeOffset? OriginalEnd { get; private set; }
        public EstimatedTimeChangedEventData(OrderBaseEntity entity, DateTimeOffset? s, DateTimeOffset? e) : base(entity)
        {
            OriginalStart = s;
            OriginalEnd = e;
        }
    }

    public class UrgencyDegreeChangedEventData : EntityEventData<OrderBaseEntity>
    {
        public UrgencyDegree Original { get; private set; }
        public UrgencyDegreeChangedEventData(OrderBaseEntity entity, UrgencyDegree original) : base(entity)
        {
            Original = original;
        }
    }

}
