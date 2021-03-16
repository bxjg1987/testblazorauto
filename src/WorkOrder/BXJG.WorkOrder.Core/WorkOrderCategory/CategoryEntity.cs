using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class CategoryEntity : GeneralTreeEntity<CategoryEntity>, IGeneratesDomainEvents
    {
        public virtual ICollection<IEventData> DomainEvents { get; } = new List<IEventData>();
    }
}
