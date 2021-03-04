using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class StatusChangedEventData : EntityEventData<OrderEntity>
    {
        public Status Original { get; private set; }
        public StatusChangedEventData(OrderEntity entity, Status original) : base(entity)
        {
            Original = original;
        }
    }
}
