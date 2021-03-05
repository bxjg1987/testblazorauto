using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.UI;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 抽象的工单聚合根，不同类型的工单应该定义相应子类
    /// </summary>
    public abstract class OrderEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 给ef用的
        /// </summary>
        private OrderEntity() { }
        /// <summary>
        /// 实例化工单
        /// </summary>
        /// <param name="categoryId">所属分类Id</param>
        /// <param name="urgencyDegree">紧急程度</param>
        /// <param name="title">标题</param>
        /// <param name="description">内容描述</param>
        /// <param name="estimatedExecutionTime">希望的开始时间</param>
        /// <param name="estimatedCompletionTime">希望的结束时间</param>
        /// <param name="extendedField1">扩展字段</param>
        /// <param name="extendedField2"></param>
        /// <param name="extendedField3"></param>
        /// <param name="extendedField4"></param>
        /// <param name="extendedField5"></param>
        public OrderEntity(long categoryId,
                           UrgencyDegree urgencyDegree,
                           string title,
                           string description = default,
                           DateTimeOffset? estimatedExecutionTime = default,
                           DateTimeOffset? estimatedCompletionTime = default,
                           string extendedField1 = default,
                           string extendedField2 = default,
                           string extendedField3 = default,
                           string extendedField4 = default,
                           string extendedField5 = default)
        {
            CategoryId = categoryId;
            //部分赋值不要用属性，以免引起事件触发
            this.status = Status.ToBeConfirm;
            this.urgencyDegree = urgencyDegree;
            this.title = title;
            Description = description;
            ChangeEstimatedTime(estimatedExecutionTime, estimatedCompletionTime);
            ExtendedField1 = extendedField1;
            ExtendedField2 = extendedField2;
            ExtendedField3 = extendedField3;
            ExtendedField4 = extendedField4;
            ExtendedField5 = extendedField5;
        }
        long categoryId;
        /// <summary>
        /// 所属分类Id
        /// </summary>
        public long CategoryId
        {
            get
            {
                return categoryId;
            }
            set
            {
                if (value <= 0)
                    throw new ApplicationException("工单分类设置无效！");
                categoryId = value;
            }
        }
        Status status;
        /// <summary>
        /// 状态
        /// </summary>
        public Status Status
        {
            get { return status; }
            private set
            {
                if (value != status)
                {
                    var o = status;
                    status = value;
                    DomainEvents.Add(new StatusChangedEventData(this, o));
                }
            }
        }
        UrgencyDegree urgencyDegree;
        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgencyDegree UrgencyDegree
        {
            get { return urgencyDegree; }
            set
            {
                if (Status == Status.Completed)
                {
                    throw new UserFriendlyException("已完成的工单不允许修改紧急程度！");
                }
                if (value != urgencyDegree)
                {
                    var o = value;
                    urgencyDegree = value;
                    DomainEvents.Add(new UrgencyDegreeChangedEventData(this, o));
                }
            }
        }
        string title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException();

                if (title != value)
                {
                    var o = title;
                    title = value;
                    //通知冗余字段更新
                    DomainEvents.Add(new TitleChangedEventData(this, o));
                }
            }
        }
        /// <summary>
        /// 内容描述
        /// </summary>
        public string Description { get; set; }
        string statusDescription;
        /// <summary>
        /// 当前状态情况说明
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return statusDescription;
            }
            set
            {
                //if (Status != Status.Completed)
                //    throw new UserFriendlyException("未完成工单不允许设置完成情况！");
                statusDescription = value;
            }
        }

        //string rejectDescription;
        ///// <summary>
        ///// 拒绝说明
        ///// </summary>
        //public string RejectDescription
        //{
        //    get => rejectDescription;
        //    set
        //    {
        //        if (Status != Status.Rejected)
        //            throw new UserFriendlyException("已拒绝的工单才能设置【拒绝理由】");
        //        rejectDescription = value;
        //    }
        //}

        /// <summary>
        /// 希望的开始时间
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTime { get; private set; }
        /// <summary>
        /// 希望的结束时间
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTime { get; private set; }

        /// <summary>
        /// 实际的执行时间
        /// </summary>
        public DateTimeOffset? ExecutionTime { get; private set; }
        /// <summary>
        /// 实际的结束时间
        /// </summary>
        public DateTimeOffset? CompletionTime { get; private set; }

        public string EmployeeId { get; private set; }
        //冗余字段通过事件修改
        public string EmployeeName { get; set; }

        public int TenantId { get; set; }
        public byte[] RowVersion { get; }
        public string ExtensionData { get; set; }
        public string ExtendedField1 { get; set; }
        public string ExtendedField2 { get; set; }
        public string ExtendedField3 { get; set; }
        public string ExtendedField4 { get; set; }
        public string ExtendedField5 { get; set; }

        /// <summary>
        /// 一并设置希望的开始和结束时间
        /// </summary>
        /// <param name="s">希望的开始时间</param>
        /// <param name="e">希望的结束时间</param>
        public void ChangeEstimatedTime(DateTimeOffset? s, DateTimeOffset? e)
        {
            if (s.HasValue && e.HasValue && s >= e)
            {
                throw new UserFriendlyException("希望的开始时间必须小于结束时间");
            }
            EstimatedExecutionTime = s;
            EstimatedCompletionTime = e;
        }
        /// <summary>
        /// 一并设置实际的开始和结束时间
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void ChangePracticalTime(DateTimeOffset? s, DateTimeOffset? e)
        {
            if (s.HasValue && e.HasValue && s >= e)
            {
                throw new UserFriendlyException("实际的开始时间必须小于结束时间");
            }
            ExecutionTime = s;
            CompletionTime = e;
        }
        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employeeName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void Distribute(string employeeId, string employeeName, DateTimeOffset? start = default, DateTimeOffset? end = default)
        {
            //各种判断
            if ((int)Status > (int)Status.ToBeProcess)
                throw new UserFriendlyException("当前状态不允许分配操作");

            EmployeeId = employeeId;
            EmployeeName = employeeName;
            Status = Status.ToBeProcess;
            DateTimeOffset? s = EstimatedExecutionTime;
            DateTimeOffset? e = EstimatedCompletionTime;
            if (start.HasValue)
                s = start;
            if (end.HasValue)
                e = end;
            ChangeEstimatedTime(s, e);
        }
        /// <summary>
        /// 执行工单
        /// </summary>
        /// <param name="time"></param>
        public void Execute(DateTimeOffset time, bool checkStatus = true)
        {
            if (checkStatus)
            {
                //业务判断
                if (Status != Status.ToBeProcess)
                    throw new UserFriendlyException("状态异常！");
            }

            //ExecutionTime = time;
            ChangePracticalTime(time, CompletionTime);
            Status = Status.Processing;
        }
        /// <summary>
        /// 完成工单
        /// </summary>
        /// <param name="time">完成时间</param>
        /// <param name="desc">完成情况说明，也可用直接设置CompletionDescription属性修改说明</param>
        /// <param name="checkStatus">是否检查工单当前状态，若不检查则任何状态的工单都可以立即设置为完成</param>
        public void Completion(DateTimeOffset time, string desc = default, bool checkStatus = true)
        {
            if (checkStatus && Status != Status.Processing)
            {
                throw new UserFriendlyException("只有处理中的工单才可以执行【完成】操作");
            }

            ChangePracticalTime(ExecutionTime, time);
            Status = Status.Completed;
            StatusDescription = desc;
        }
        /// <summary>
        /// 拒绝<br />
        /// 任何状态下的工单都可以直接拒绝，拒绝说明必须录入
        /// </summary>
        public void Reject(string desc)
        {
            Status = Status.Rejected;
            StatusDescription = desc;
        }

        //public virtual OrderEntity Copy()
        //{
        //    return new OrderEntity();
        //}

        //这种情况直接拒绝复制一个就好了
        ///// <summary>
        ///// 重置<br />
        ///// 重新回到初始创建状态
        ///// </summary>
        //public void Reset()
        //{
        //    completionDescription = default;
        //    rejectDescription = default;
        //    ChangeEstimatedTime(default, default);
        //    ChangePracticalTime(default, default);
        //    status = Status.ToBeConfirm;
        //}
    }
}
