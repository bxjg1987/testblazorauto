using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.BaseInfo.StaffInfo;
using BXJG.WorkOrder.WorkOrder;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using System.Linq;

namespace ZLJ.WorkOrder.RentOrderItemWorkOrder
{
    /// <summary>
    /// 与租赁单明细关联的工单实体
    /// </summary>
    public class RentOrderItemWorkOrderEntity : WorkOrderBaseEntity
    {
        private RentOrderItemWorkOrderEntity() : base() { }

        public RentOrderItemWorkOrderEntity(DateTimeOffset time,
                                            long categoryId,
                                            string title,
                                            long rentOrderItemId,
                                            string description = null,
                                            UrgencyDegree urgencyDegree = UrgencyDegree.Normalize,
                                            string employeeId = null,
                                            DateTimeOffset? estimatedExecutionTime = null,
                                            DateTimeOffset? estimatedCompletionTime = null) : base(time,
                                                                                                   categoryId,
                                                                                                   title,
                                                                                                   description,
                                                                                                   urgencyDegree,
                                                                                                   employeeId,
                                                                                                   estimatedExecutionTime,
                                                                                                   estimatedCompletionTime)
        {
            this.RentOrderItemId = rentOrderItemId;
        }

        /// <summary>
        /// 关联的租单明细Id
        /// </summary>

        public long rentOrderItemId;
        [DisableAuditing]
        public long RentOrderItemId
        {
            get { return rentOrderItemId; }
            set { rentOrderItemId = value; UpdateUnicodeToken(); }

        }
        /// <summary>
        /// 关联的租赁单明细
        /// </summary>
        public virtual ZLJ.Rent.Order.OrderItemEntity RentOrderItem { get; set; }

        protected override OrderBaseEntity CopyCreate()
        {
            return new RentOrderItemWorkOrderEntity(StatusChangedTime,
                                                    CategoryId,
                                                    Title,
                                                    RentOrderItemId,
                                                    Description,
                                                    UrgencyDegree,
                                                    EmployeeId,
                                                    EstimatedExecutionTime,
                                                    EstimatedCompletionTime);
        }

     
        public string UnicodeToken { get; private set; }
        internal protected virtual void UpdateUnicodeToken()
        {
            var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
            if (pc.Any(c => c == this.Status))
                UnicodeToken = this.Id.ToString();
            else
                UnicodeToken = this.Id + "-" + this.RentOrderItemId;
        }

        public override Status Status
        {
            get { return base.Status; }
            protected set
            {
                base.Status = value;
                UpdateUnicodeToken();
            }
        }
    }
}
