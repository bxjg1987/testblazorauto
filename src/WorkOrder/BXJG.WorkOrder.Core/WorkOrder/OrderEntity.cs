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
                           DateTimeOffset? expectedExecutionTime,
                           DateTimeOffset? expectedCompletionTime)
        {
            CategoryId = categoryId;
            Status = Status.PendingApproval;
            UrgencyDegree = urgencyDegree;
            ExpectedExecutionTime = expectedExecutionTime;
            ExpectedCompletionTime = expectedCompletionTime;
        }

        public long CategoryId { get; set; }
        public Status Status { get; private set; }
        public UrgencyDegree UrgencyDegree { get; set; }
        public DateTimeOffset? ExpectedExecutionTime { get; set; }
        public DateTimeOffset? ExpectedCompletionTime { get; set; }
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

        public void Distribute(string employeeId, string employeeName, DateTimeOffset time)
        {
            //各种判断
            if (Status != Status.PendingApproval)
                throw new UserFriendlyException();

            EmployeeId = employeeId;
            EmployeeName = employeeName;
            Status = Status.Processing;
        }
    }
}
