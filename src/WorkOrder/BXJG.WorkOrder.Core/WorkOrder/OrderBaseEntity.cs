using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.UI;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 抽象的工单聚合根，不同类型的工单应该定义相应子类
    /// </summary>
    public abstract class OrderBaseEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 给ef用的
        /// </summary>
        protected internal OrderBaseEntity() { }
        /// <summary>
        /// 实例化工单
        /// </summary>
        /// <param name="categoryId">所属分类Id</param>
        /// <param name="urgencyDegree">紧急程度</param>
        /// <param name="title">标题</param>
        /// <param name="time">创建此对象的时间</param>
        /// <param name="description">内容描述</param>
        /// <param name="estimatedExecutionTime">希望的开始时间</param>
        /// <param name="estimatedCompletionTime">希望的结束时间</param>
        /// <param name="extendedField1">扩展字段</param>
        /// <param name="extendedField2"></param>
        /// <param name="extendedField3"></param>
        /// <param name="extendedField4"></param>
        /// <param name="extendedField5"></param>
        protected internal OrderBaseEntity(long categoryId,
                                           UrgencyDegree urgencyDegree,
                                           string title,
                                           DateTimeOffset time,
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
            Status = Status.ToBeConfirmed;
            this.urgencyDegree = urgencyDegree;
            this.title = title;
            this.StatusChangedTime = time;
            Description = description;
            ChangeEstimatedTime(estimatedExecutionTime, estimatedCompletionTime);
            ExtendedField1 = extendedField1;
            ExtendedField2 = extendedField2;
            ExtendedField3 = extendedField3;
            ExtendedField4 = extendedField4;
            ExtendedField5 = extendedField5;
        }
        protected long categoryId;
        /// <summary>
        /// 所属分类Id
        /// </summary>
        public virtual long CategoryId
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
        /// <summary>
        /// 状态
        /// </summary>
        public virtual Status Status { get; protected set; }
        protected UrgencyDegree urgencyDegree;
        /// <summary>
        /// 紧急程度
        /// </summary>
        public virtual UrgencyDegree UrgencyDegree
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
        protected string title;
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title
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
        public virtual string Description { get; set; }
        /// <summary>
        /// 当前状态情况说明
        /// </summary>
        public virtual string StatusChangedDescription { get; protected set; }
        /// <summary>
        /// 变成当前状态的时间
        /// </summary>
        public virtual DateTimeOffset StatusChangedTime { get; protected set; }

        /// <summary>
        /// 希望的开始时间
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTime { get; protected set; }
        /// <summary>
        /// 希望的结束时间
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTime { get; protected set; }

        /// <summary>
        /// 实际的执行时间
        /// </summary>
        public virtual DateTimeOffset? ExecutionTime { get; protected set; }
        /// <summary>
        /// 实际的结束时间
        /// </summary>
        public virtual DateTimeOffset? CompletionTime { get; protected set; }

        public virtual string EmployeeId { get; protected set; }
        ///// <summary>
        ///// 联系人
        ///// </summary>
        //public virtual string ContactName { get; set; }
        ///// <summary>
        ///// 联系电话
        ///// </summary>
        //public virtual string ContactPhone { get; set; }

        public virtual int TenantId { get; set; }
        /// <summary>
        /// 状态变更有并发可能，使用乐观并发，偷个懒直接使用行级乐观并发
        /// </summary>
        public virtual byte[] RowVersion { get; }
        public virtual string ExtensionData { get; set; }
        public virtual string ExtendedField1 { get; set; }
        public virtual string ExtendedField2 { get; set; }
        public virtual string ExtendedField3 { get; set; }
        public virtual string ExtendedField4 { get; set; }
        public virtual string ExtendedField5 { get; set; }

        /// <summary>
        /// 调整状态
        /// </summary>
        /// <param name="status">目标状态</param>
        /// <param name="time">时间</param>
        /// <param name="desc">描述</param>
        protected virtual void ChangeStatus(Status status, DateTimeOffset time, string desc = default)
        {
            var o = this.Status;
            Status = status;
            StatusChangedTime = time;
            StatusChangedDescription = desc;
            DomainEvents.Add(new StatusChangedEventData(this, o));
        }
        /// <summary>
        /// 一并设置希望的开始和结束时间
        /// </summary>
        /// <param name="s">希望的开始时间</param>
        /// <param name="e">希望的结束时间</param>
        public virtual void ChangeEstimatedTime(DateTimeOffset? s, DateTimeOffset? e)
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
        /// <param name="s">实际开始时间</param>
        /// <param name="e">实际结束时间</param>
        public virtual void ChangePracticalTime(DateTimeOffset? s, DateTimeOffset? e)
        {
            if (s.HasValue && e.HasValue && s >= e)
            {
                throw new UserFriendlyException("实际的开始时间必须小于结束时间");
            }
            ExecutionTime = s;
            CompletionTime = e;
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="time"></param>
        public virtual void Confirme(DateTimeOffset time)
        {
            if (Status != Status.ToBeConfirmed)
                throw new UserFriendlyException("状态异常！");

            ChangeStatus(Status.ToBeAllocated, time);
        }
        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="time">分配时间时间</param>
        /// <param name="employeeId">员工id</param>
        /// <param name="start">希望的开始时间</param>
        /// <param name="end">希望的结束时间</param>
        public virtual void Allocate(DateTimeOffset time, string employeeId, DateTimeOffset? start = default, DateTimeOffset? end = default)
        {
            if (Status != Status.ToBeAllocated)
            {
                throw new UserFriendlyException("当前状态不允许分配操作");
            }

            EmployeeId = employeeId;
            DateTimeOffset? s = start ?? EstimatedExecutionTime;
            DateTimeOffset? e = end ?? EstimatedCompletionTime;
            ChangeEstimatedTime(s, e);
            ChangeStatus(Status.ToBeProcessed, time);
        }
        /// <summary>
        /// 执行工单
        /// </summary>
        /// <param name="time"></param>
        public virtual void Execute(DateTimeOffset time)
        {
            if (Status != Status.ToBeProcessed)
            {
                throw new UserFriendlyException("状态异常！");
            }

            ChangePracticalTime(time, CompletionTime);
            ChangeStatus(Status.Processing, time);
        }
        /// <summary>
        /// 完成工单
        /// </summary>
        /// <param name="time">完成时间</param>
        /// <param name="desc">完成情况说明，也可用直接设置CompletionDescription属性修改说明</param>
        public virtual void Completion(DateTimeOffset time, string desc)
        {
            if (Status != Status.Processing)
            {
                throw new UserFriendlyException("只有处理中的工单才可以执行【完成】操作");
            }
            ChangePracticalTime(ExecutionTime, time);
            ChangeStatus(Status.Completed, time, desc);
        }
        /// <summary>
        /// 拒绝<br />
        /// 任何状态下的工单都可以直接拒绝
        /// </summary>
        /// <param name="time">操作时间</param>
        /// <param name="desc">拒绝原因</param>
        public virtual void Reject(DateTimeOffset time, string desc = "拒绝")
        {
            ChangeStatus(Status.Rejected, time, desc);
        }
        /// <summary>
        /// 回退到指定状态
        /// </summary>
        /// <param time="time"></param>
        /// <param name="status"></param>
        /// <param desc="desc"></param>
        public virtual void BackOff(DateTimeOffset time, Status status = Status.ToBeConfirmed, string desc = "回退")
        {
            var i = (int)status;
            if (i <= (int)Status)
            {
                return;
                //throw new ApplicationException("此状态不允许【回退】操作！");
            }
            if (i <= (int)Status.Processing)
            {
                ChangePracticalTime(ExecutionTime, default);
            }
            if (i <= (int)Status.ToBeProcessed)
            {
                ChangePracticalTime(default, default);
            }
            if (i <= (int)Status.ToBeAllocated)
            {
                EmployeeId = default;
            }
            if (status == Status.ToBeConfirmed)
            {
                //EmployeeId = default;
                //EmployeeName = default;
            }
            ChangeStatus(status, time, desc);
        }
        /// <summary>
        /// 复制工单时创建工单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract OrderBaseEntity CopyCreate();
        /// <summary>
        /// <summary>
        /// 复制工单
        /// </summary>
        /// <typeparam name="T">具体的工单类型，OrderEntity的子类</typeparam>
        /// <param name="time">复制操作的时间</param>
        /// <param name="status">复制后的工单希望处于什么状态</param>
        /// <param name="desc">此操作的原因</param>
        /// <returns></returns>
        public virtual T Copy<T>(DateTime time, Status status = Status.ToBeConfirmed, string desc = "复制") where T : OrderBaseEntity
        {
            var entity = CopyCreate() as T;
            entity.BackOff(time, status, desc);
            return entity;
        }
    }
    /// <summary>
    /// 普通的默认的gd
    /// </summary>
    public class OrderEntity : OrderBaseEntity
    {
        protected internal OrderEntity() : base() { }
        protected internal OrderEntity(long categoryId,
                                       UrgencyDegree urgencyDegree,
                                       string title,
                                       DateTimeOffset time,
                                       string description = null,
                                       DateTimeOffset? estimatedExecutionTime = null,
                                       DateTimeOffset? estimatedCompletionTime = null,
                                       string extendedField1 = null,
                                       string extendedField2 = null,
                                       string extendedField3 = null,
                                       string extendedField4 = null,
                                       string extendedField5 = null) : base(categoryId,
                                                                            urgencyDegree,
                                                                            title,
                                                                            time,
                                                                            description,
                                                                            estimatedExecutionTime,
                                                                            estimatedCompletionTime,
                                                                            extendedField1,
                                                                            extendedField2,
                                                                            extendedField3,
                                                                            extendedField4,
                                                                            extendedField5)
        {
        }

        protected override OrderBaseEntity CopyCreate()
        {
            return new OrderEntity(CategoryId,
                                   UrgencyDegree,
                                   Title,
                                   StatusChangedTime,
                                   Description,
                                   EstimatedExecutionTime,
                                   EstimatedCompletionTime,
                                   ExtendedField1,
                                   ExtendedField2,
                                   ExtendedField3,
                                   ExtendedField4,
                                   ExtendedField5);
        }

    }
}