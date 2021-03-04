using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.UI;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    public abstract class OrderEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        private OrderEntity() { }
        public OrderEntity(long categoryId,
                           UrgencyDegree urgencyDegree,
                           DateTimeOffset? estimatedExecutionTime = default,
                           DateTimeOffset? estimatedCompletionTime = default)
        {
            CategoryId = categoryId;
            Status = Status.ToBeConfirmed;
            UrgencyDegree = urgencyDegree;
            EstimatedExecutionTime = estimatedExecutionTime;
            EstimatedCompletionTime = estimatedCompletionTime;
        }

        public long CategoryId { get; set; }
        public Status Status { get; private set; }
        public UrgencyDegree UrgencyDegree { get; set; }
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
        public DateTimeOffset? ExecutionTime { get; private set; }
        public DateTimeOffset? CompletionTime { get; private set; }

        public string EmployeeId { get; private set; }
        public string EmployeeName { get; private set; }

        public int TenantId { get; set; }
        public byte[] RowVersion { get; }
        public string ExtensionData { get; set; }
        public string ExtendedField1 { get; set; }
        public string ExtendedField2 { get; set; }
        public string ExtendedField3 { get; set; }
        public string ExtendedField4 { get; set; }
        public string ExtendedField5 { get; set; }

        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employeeName"></param>
        /// <param name="time"></param>
        public void Distribute(string employeeId, string employeeName, DateTimeOffset time)
        {
            //各种判断
            if ((int)Status.ToBeProcessed > 1)
                throw new UserFriendlyException("状态异常");

            EmployeeId = employeeId;
            EmployeeName = employeeName;
            Status = Status.Processing;
            ExecutionTime = time;
        }
    }
}
