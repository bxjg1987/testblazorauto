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
    /// <summary>
    /// 工单分类实体。业务简单，不使用充血模型
    /// </summary>
    public class CategoryEntity : GeneralTreeEntity<CategoryEntity>, IGeneratesDomainEvents
    {
        public virtual ICollection<IEventData> DomainEvents { get; } = new List<IEventData>();
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
        public virtual ICollection<CategoryWorkOrderTypeEntity> WorkOrderTypes { get; set; }
        /// <summary>
        /// 是否为默认类别
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
