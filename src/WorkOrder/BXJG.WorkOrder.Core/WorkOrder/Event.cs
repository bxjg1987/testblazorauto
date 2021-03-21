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
    //public class TitleChangedEventData : EntityEventData<OrderBaseEntity>
    //{
    //    public string Original { get; private set; }
    //    public TitleChangedEventData(OrderBaseEntity entity, string original) : base(entity)
    //    {
    //        Original = original;
    //    }
    //}

    public class UrgencyDegreeChangedEventData : EntityEventData<OrderBaseEntity>
    {
        public UrgencyDegree Original { get; private set; }
        public UrgencyDegreeChangedEventData(OrderBaseEntity entity, UrgencyDegree original) : base(entity)
        {
            Original = original;
        }
    }
    
}
