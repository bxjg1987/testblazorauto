using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class StatusChangeingEventData : EntityChangingEventData<OrderBaseEntity>
    {
        public Status Original { get; private set; }
        public StatusChangeingEventData(OrderBaseEntity entity, Status original) : base(entity)
        {
            Original = original;
        }
    }
    ////通知冗余字段引用方修改自己的数据
    public class EstimatedTimeChangeingEventData : EntityChangingEventData<OrderBaseEntity>
    {
        public DateTimeOffset? OriginalStart { get; private set; }
        public DateTimeOffset? OriginalEnd { get; private set; }
        public EstimatedTimeChangeingEventData(OrderBaseEntity entity, DateTimeOffset? s, DateTimeOffset? e) : base(entity)
        {
            OriginalStart = s;
            OriginalEnd = e;
        }
    }

    public class UrgencyDegreeChangingEventData : EntityChangingEventData<OrderBaseEntity>
    {
        public UrgencyDegree Original { get; private set; }
        public UrgencyDegreeChangingEventData(OrderBaseEntity entity, UrgencyDegree original) : base(entity)
        {
            Original = original;
        }
    }

    public class PointsChangingEventData : EntityChangingEventData<OrderBaseEntity>
    {
        public int? Original { get; private set; }
        public PointsChangingEventData(OrderBaseEntity entity, int? original) : base(entity)
        {
            Original = original;
        }
    }
}
