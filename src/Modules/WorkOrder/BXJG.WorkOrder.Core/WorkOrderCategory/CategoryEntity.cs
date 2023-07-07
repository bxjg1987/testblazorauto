using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;
using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单分类实体。业务简单，不使用充血模型
    /// </summary>
    public class CategoryEntity : GeneralTreeEntity<CategoryEntity>//, IGeneratesDomainEvents
    {
        ///// <summary>
        ///// 领域事件
        ///// </summary>
        //public virtual ICollection<IEventData> DomainEvents { get; } = new List<IEventData>();
        /// <summary>
        /// 是否默认
        /// </summary>
        public virtual bool IsDefault { get; set; }
        /// <summary>
        /// 哪些类型的工单可以共用这个类别
        /// </summary>
        public virtual ICollection<WorkOrderCategoryTypeEntity> WorkOrderTypes { get; set; } = new Collection<WorkOrderCategoryTypeEntity>();
    }
}
