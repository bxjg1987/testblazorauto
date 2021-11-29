using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using Abp.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using BXJG.Utils.Extensions;

namespace BXJG.WorkOrder.WorkOrder
{
    /*
     * 在录入信息时肯定有人为录入错误的情况，最直观的想法就是修改对应的属性，那么就需要将所有属性定义为public set，这不合理
     * 因此，考虑属性时候可写时，不考虑认为录入错误需要直接修改的情况
     * 
     * ************************************************************************************
     * 
     * 在调用分配方法时，可能工单已处于已分配状态，此时可以retrun或抛出错误，具体用哪种方式呢？
     * 如果直接返回，相关参数如 分配给谁的，预计开始和结束时间不会得到更新，而调用方会以为正常赋值了。
     * 若抛出异常，则更符合逻辑，但调用方也许只是想尝试调用，若已分配则不做任何处理
     * 这是一种普遍情况，确认、分配、执行、完成都有类似的情况
     * 这种问题在属性赋值中不存在，因为没有参数
     * 
     * 方式1
     * 在实体中抛出异常，这样更符合真实业务逻辑
     * 定义扩展方法，先判断状态是否有变化，再决定是否调用
     * 
     * 方式2 
     * 反过来，在实体中直接返回，在扩展方法中判断抛出异常
     * 
     * 方式3
     * 在实体方法中增加可选参数来决定
     * 
     * 目前采用更严谨的方式1（如分配ChangeStatus方法），但无争议的还是使用return（如ChangeEstimatedTime方法）
     * 
     * ************************************************************************************
     * 
     * 实体类中的本地化不好处理，目前使用的是简单的扩展方法形式，简单，但不便于单元测试
     * 应用程序已经可以暂时不考虑本地化
     * 
     * ************************************************************************************
     * 某个状态的工单不允许执行某个状态，将抛出异常
     * 当调用方严格按正常的业务调用时永远不会抛出异常，这种情况用Exception还是UserFriendlyException都一样
     * 当调用方没有严格按正常的业务调用时，抛出UserFriendlyException更合适。
     * 
     */

    /// <summary>
    /// 抽象的工单实体，它是聚合根，不同类型的工单应该定义相应子类
    /// 参考：https://gitee.com/bxjg1987_admin/abp/wikis/pages?sort_id=3712351&doc_id=627313
    /// </summary>
    [Audited]
    public abstract class OrderBaseEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        public static readonly UrgencyDegree DefaultUrgencyDegree = UrgencyDegree.Normalize;

        /// <summary>
        /// 给ef用的，同时允许子类重写
        /// </summary>
        protected OrderBaseEntity() { }
        /// <summary>
        /// 实例化工单的抽象构造函数
        /// </summary>
        /// <param name="time">创建此对象的时间，因为初始状态需要一个时间</param>
        /// <param name="categoryId">所属分类Id，若不确定也应该提供一个默认类别</param>
        /// <param name="title">标题，不允许为空</param>
        /// <param name="description">工单内容说明</param>
        /// <param name="urgencyDegree">紧急程度，默认为<see cref="UrgencyDegree.Normalize"/></param>
        /// <param name="employeeId">员工id，可以预先指定，也可以在<see cref="Allocate"/>时指定</param>
        /// <param name="estimatedExecutionTime">预计开始时间，可以预先指定，也可以在<see cref="Allocate"/>时指定</param>
        /// <param name="estimatedCompletionTime">预计结束时间，可以预先指定，也可以在<see cref="Allocate"/>时指定</param>
        protected internal OrderBaseEntity(DateTimeOffset time,
                                           long categoryId,
                                           string title,
                                           string description = default,
                                           UrgencyDegree urgencyDegree = UrgencyDegree.Normalize,
                                           string employeeId = default,
                                           DateTimeOffset? estimatedExecutionTime = default,
                                           DateTimeOffset? estimatedCompletionTime = default)
        {
            CategoryId = categoryId;
            this.urgencyDegree1 = urgencyDegree;
            Title = title;

            Status = Status.ToBeConfirmed;
            StatusChangedTime = time;
            Description = description;

            EmployeeId = employeeId;
            ChangeEstimatedTime(estimatedExecutionTime, estimatedCompletionTime);
        }
        /// <summary>
        /// 所属分类id。子类需要访问时请尽量调用<see cref="CategoryId"/>
        /// </summary>
        protected long categoryId;
        /// <summary>
        /// 所属分类Id。无法提供时也应该给个默认值
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
                    throw new ApplicationException("工单分类设置无效！");//应用程序异常目前不考虑本地化
                categoryId = value;
            }
        }

        private object lockerzt = new object();
        private Status? yszt;
        private Status status1;
        /// <summary>
        /// 状态。核心属性，调用相应业务方法可以改变其值
        /// </summary>
        public virtual Status Status
        {
            get { return status1; }
            protected set
            {
                if (!yszt.HasValue)
                {
                    lock (lockerzt)
                    {
                        if (!yszt.HasValue)
                            yszt = value;
                    }
                }

                //确保事件与状态同步
                lock (lockerzt)
                {
                    status1 = value;
                    if (value != yszt.Value)
                        DomainEvents.AddOrReplace(new StatusChangeingEventData(this, yszt.Value));
                    else
                        DomainEvents.Remove<StatusChangeingEventData>();
                }
            }
        }

        private object lockerDegreeysz = new object();
        private UrgencyDegree? urgencyDegreeysz;
        /// <summary>
        /// 紧急程度。子类需要访问时请尽量调用<see cref="UrgencyDegree"/>
        /// </summary>
        private UrgencyDegree urgencyDegree1;
        /// <summary>
        /// 紧急程度。已完成或拒绝的工单不允许修改，其它状态可以修改
        /// 修改后将触发<see cref="UrgencyDegreeChangingEventData"/>事件
        /// </summary>
        public virtual UrgencyDegree UrgencyDegree
        {
            get { return urgencyDegree1; }
            set
            {
                if (Status >= Status.Completed)
                    throw new UserFriendlyException("workorderSetUrgencyDegreeStatusException".BXJGWorkOrderL());

                if (!urgencyDegreeysz.HasValue)
                {
                    lock (lockerDegreeysz)
                    {
                        if (!urgencyDegreeysz.HasValue)
                            urgencyDegreeysz = value;
                    }
                }
                //确保事件与状态同步
                lock (lockerDegreeysz)
                {
                    urgencyDegree1 = value;
                    if (value != urgencyDegreeysz.Value)
                        DomainEvents.Add(new UrgencyDegreeChangingEventData(this, urgencyDegreeysz.Value));
                    else
                        DomainEvents.Remove<UrgencyDegreeChangingEventData>();
                }
            }
        }
        /// <summary>
        /// 工单标题。子类需要访问时请尽量调用<see cref="Title"/>
        /// </summary>
        protected string titie;
        /// <summary>
        /// 标题。
        /// </summary>
        [DisableAuditing]
        public virtual string Title
        {
            get { return titie; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Title));//应用程序异常目前不考虑本地化
                }
                titie = value;
            }
        }
        /// <summary>
        /// 内容描述
        /// </summary>
        [DisableAuditing]
        public virtual string Description { get; set; }
        ///// <summary>
        ///// 关联的原始图片列表，多个用英文逗号分割
        ///// 缩略图与原始图片是按顺序关联的
        ///// 存储可访问路径，而不是相对路径
        ///// </summary>
        //public virtual string Images { get; set; }
        ///// <summary>
        ///// 关联的缩略图列表，多个用英文逗号分割
        ///// 缩略图与原始图片是按顺序关联的
        ///// 存储可访问路径，而不是相对路径
        ///// </summary>
        //public virtual string ThumImages { get; set; }

        /// <summary>
        /// 当前状态情况说明。相关业务方法的description参数将复制给此属性
        /// </summary>
        public virtual string StatusChangedDescription { get; set; }
        /// <summary>
        /// 变成当前状态的时间。相关业务方法的time参数将复制给此属性
        /// </summary>
        public virtual DateTimeOffset StatusChangedTime { get; protected set; }
        /// <summary>
        /// 预计开始时间。参考：<see cref="ChangeEstimatedTime"/>
        /// </summary>

        private bool yjkscsh = false, yjjscsh = false;
        private DateTimeOffset? estimatedExecutionTimeysz, estimatedCompletionTimeysz;

        private DateTimeOffset? estimatedExecutionTime1, estimatedCompletionTime1;

        private object lockeryjks = new object();
        private object lockeryjwc = new object();
        public virtual DateTimeOffset? EstimatedExecutionTime
        {
            get { return estimatedExecutionTime1; }
            set
            {
                estimatedExecutionTime1 = value;
                if (!yjkscsh)
                {
                    lock (lockeryjks)
                    {
                        if (!yjkscsh)
                        {
                            estimatedExecutionTimeysz = value;
                            yjkscsh = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 预计结束时间。参考：<see cref="ChangeEstimatedTime"/>
        /// </summary>

        public virtual DateTimeOffset? EstimatedCompletionTime
        {
            get { return estimatedCompletionTime1; }
            set
            {
                estimatedCompletionTime1 = value;
                if (!yjjscsh)
                {
                    lock (lockeryjwc)
                    {
                        if (!yjjscsh)
                        {
                            estimatedCompletionTimeysz = value;
                            yjjscsh = true;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 执行时间。参考：<see cref="ChangePracticalTime"/>
        /// </summary>
        public virtual DateTimeOffset? ExecutionTime { get; protected set; }
        /// <summary>
        /// 完成时间。参考：<see cref="ChangePracticalTime"/>
        /// </summary>
        public virtual DateTimeOffset? CompletionTime { get; protected set; }


        /// <summary>
        /// 处理人Id。子类需要访问时请尽量调用<see cref="EmployeeId"/>
        /// </summary>
        protected string employeeId;
        /// <summary>
        /// 负责处理此工单的员工id
        /// 未分配之前可以设置为任意员工
        /// 已分配后员工可能已经在前往处理工单的途中，此时不允许修改此属性，应该走回退或移交工单的流程
        /// 设置此属性只是表示准备将此工单分配给该员工，而不是真实分配给他，工单状态不会改变，真实的分配操作请调用<see cref="Allocate"/>
        /// </summary>
        public virtual string EmployeeId
        {
            get { return employeeId; }
            set
            {
                if (value == employeeId)
                    return;
                if (Status >= Status.ToBeProcessed)
                    throw new UserFriendlyException("workorderSetEmployeeStatusException".BXJGWorkOrderL());
                employeeId = value;
            }
        }
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
        //[DisableAuditing]
        //只给状态加并发控制
        //public byte[] RowVersion { get; set; }
        /// <summary>
        /// 调整状态。将引发<see cref="StatusChangeingEventData"/>事件
        /// </summary>
        /// <param name="status">目标状态</param>
        /// <param name="time">时间</param>
        /// <param name="description">描述</param>
        protected virtual void ChangeStatus(Status status, DateTimeOffset time, string description = default)
        {
            //此方法是内部的，服务于各业务操作的辅助方法，各业务方法已做判断，这里不必要再判断
            //if (Status == status)
            //{
            //    //return;若return，StatusChangedTime、StatusChangedDescription将不会得到更新，但程序不会报错，会误导调用方以为这些属性是赋值成功的。
            //    throw new ApplicationException("状态无变化，不应该调用此方法！");//不使用UserFriendlyException，因为这个问题应该由开发人员处理，应用程序异常目前不考虑本地化
            //}

            StatusChangedTime = time;
            StatusChangedDescription = description;
            Status = status;
        }

        private object lockeryjkswc = new object();
        /// <summary>
        /// 一并设置预计开始和结束时间
        /// 预计开始时间必须小于等于结束时间
        /// 预计开始时间和结束时间关乎员工的工作效率，因此不允许随意修改,“执行中”之前的状态可以修改预计开始时间，“完成”之前的状态可设置预计结束时间
        /// </summary>
        /// <param name="estimatedExecutionTime">预计开始时间</param>
        /// <param name="estimatedCompletionTime">预计结束时间</param>
        public virtual void ChangeEstimatedTime(DateTimeOffset? estimatedExecutionTime = default, DateTimeOffset? estimatedCompletionTime = default)
        {
            if (estimatedExecutionTime == EstimatedExecutionTime && estimatedCompletionTime == EstimatedCompletionTime)
                return;

            if (estimatedExecutionTime.HasValue && estimatedCompletionTime.HasValue && estimatedExecutionTime > estimatedCompletionTime)
                throw new UserFriendlyException("workorderChangeEstimatedTimeException1".BXJGWorkOrderL());

            //DateTimeOffset? os = EstimatedExecutionTime, oe = EstimatedCompletionTime;

            //确保状态的改变跟事件触发是事务性的
            lock (lockeryjkswc)
            {
                if (estimatedExecutionTime != EstimatedExecutionTime)
                {
                    if (Status >= Status.Processing)
                        throw new UserFriendlyException("workorderChangeEstimatedTimeException2".BXJGWorkOrderL());

                    EstimatedExecutionTime = estimatedExecutionTime;
                }
                if (estimatedCompletionTime != EstimatedCompletionTime)
                {
                    if (Status >= Status.Completed)
                        throw new UserFriendlyException("workorderChangeEstimatedTimeException3".BXJGWorkOrderL());

                    EstimatedCompletionTime = estimatedCompletionTime;
                }

                if (estimatedCompletionTime != estimatedCompletionTimeysz || estimatedExecutionTime != estimatedExecutionTimeysz)
                    DomainEvents.AddOrReplace(new EstimatedTimeChangeingEventData(this, estimatedCompletionTimeysz, estimatedExecutionTimeysz));
                else
                    DomainEvents.Remove<EstimatedTimeChangeingEventData>();
            }
        }
        /// <summary>
        /// 一并设置执行和完成时间<br />
        /// 完成时间必须大于等于结束时间<br />
        /// 在执行或完成时才调用此方法<br />
        /// 回退时可能都设置为空
        /// </summary>
        /// <param name="executionTime">执行时间</param>
        /// <param name="completionTime">完成时间</param>
        protected virtual void ChangePracticalTime(DateTimeOffset? executionTime = default, DateTimeOffset? completionTime = default)
        {
            if (executionTime.HasValue && completionTime.HasValue && executionTime > completionTime)
                throw new UserFriendlyException("workorderChangePracticalTimeException1".BXJGWorkOrderL());

            //在执行或完成时才调用此方法，更多逻辑在那两个方法中

            ExecutionTime = executionTime;
            CompletionTime = completionTime;
        }
        /// <summary>
        /// 确认，只有待确认的工单才允许执行此操作
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void Confirme(DateTimeOffset time, string description = "确认")
        {
            if (Status != Status.ToBeConfirmed)
            {
                throw new UserFriendlyException("workorderConfirmeException1".BXJGWorkOrderL());
            }
            ChangeStatus(Status.ToBeAllocated, time, description);
        }

      

        /// <summary>
        /// 反确认
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void UnConfirme(DateTimeOffset time, string description = "反确认")
        {
            if (Status != Status.ToBeAllocated)
            {
                throw new UserFriendlyException("workorderUnConfirmeException1".BXJGWorkOrderL());
            }
            ChangeStatus(Status.ToBeConfirmed, time, description);
        }
        /// <summary>
        /// 分配，只有待分配的工单才允许执行此操作
        /// </summary>
        /// <param name="time">分配时间时间</param>
        /// <param name="employeeId">员工id，为空则表示只想记录下问题，不需要明确是谁做的</param>
        /// <param name="estimatedExecutionTime">预计开始时间</param>
        /// <param name="estimatedCompletionTime">预计结束时间</param>
        /// <param name="description"></param>
        public virtual void Allocate(DateTimeOffset time, string employeeId = default, DateTimeOffset? estimatedExecutionTime = default, DateTimeOffset? estimatedCompletionTime = default, string description = "分配")
        {
            if (Status != Status.ToBeAllocated)
            {
                throw new UserFriendlyException("workorderAllocateException1".BXJGWorkOrderL());
            }
            EmployeeId = employeeId;
            ChangeEstimatedTime(estimatedExecutionTime, estimatedCompletionTime);
            ChangeStatus(Status.ToBeProcessed, time, description);
        }
        /// <summary>
        /// 反分配
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void UnAllocate(DateTimeOffset time, string description = "反分配")
        {
            if (Status != Status.ToBeProcessed)
            {
                throw new UserFriendlyException("workorderUnAllocateException1".BXJGWorkOrderL());
            }
            //EmployeeId = default;
            //ChangeEstimatedTime(default, default);
            ChangeStatus(Status.ToBeAllocated, time, description);
        }
        /// <summary>
        /// 执行工单，只有待执行的工单才允许执行此操作
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void Execute(DateTimeOffset time, string description = "执行")
        {
            if (Status != Status.ToBeProcessed)
            {
                throw new UserFriendlyException("workorderExecuteException1".BXJGWorkOrderL());
            }
            ChangePracticalTime(time, default);
            ChangeStatus(Status.Processing, time, description);
        }
        /// <summary>
        /// 反执行
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void UnExecute(DateTimeOffset time, string description = "反执行")
        {
            if (Status != Status.Processing)
            {
                throw new UserFriendlyException("workorderUnExecuteException1".BXJGWorkOrderL());
            }
            ChangePracticalTime(default, default);
            ChangeStatus(Status.ToBeProcessed, time, description);
        }
        /// <summary>
        /// 完成工单，只有执行中的工单才允许执行此操作
        /// </summary>
        /// <param name="time">完成时间</param>
        /// <param name="description">完成情况说明，也可用直接设置CompletionDescription属性修改说明</param>
        public virtual void Completion(DateTimeOffset time, string description = "完成")
        {
            if (Status != Status.Processing)
            {
                throw new UserFriendlyException("workorderCompletionException1".BXJGWorkOrderL());
            }
            ChangePracticalTime(ExecutionTime, time);
            ChangeStatus(Status.Completed, time, description);
        }
        /// <summary>
        /// 反完成
        /// </summary>
        /// <param name="time"></param>
        /// <param name="description"></param>
        public virtual void UnCompletion(DateTimeOffset time, string description = "反完成")
        {
            if (Status != Status.Completed)
            {
                throw new UserFriendlyException("workorderUnCompletionException1".BXJGWorkOrderL());
            }
            ChangePracticalTime(ExecutionTime, default);
            ChangeStatus(Status.Processing, time, description);
        }
        /// <summary>
        /// 拒绝。
        /// “已完成”、“拒绝”状态的工单不允许执行此操作
        /// </summary>
        /// <param name="time">操作时间</param>
        /// <param name="description">原因</param>
        public virtual void Reject(DateTimeOffset time, string description = "拒绝")
        {
            if (Status >= Status.Completed)
            {
                throw new UserFriendlyException("workorderRejectException1".BXJGWorkOrderL());
            }
            ChangeStatus(Status.Rejected, time, description);
        }
        /// <summary>
        /// 反拒绝。
        /// 只有“拒绝”状态的工单才可以执行此操作
        /// 由于任意状态的工单都可以执行拒绝操作，因此反拒绝无法确认之前的状态，只能回到“待确认”状态
        /// </summary>
        /// <param name="time">操作时间</param>
        /// <param name="description">原因</param>
        public virtual void UnReject(DateTimeOffset time, string description = "反拒绝")
        {
            if (Status != Status.Rejected)
            {
                throw new UserFriendlyException("workorderUnRejectException1".BXJGWorkOrderL());
            }
            ChangeStatus(Status.ToBeConfirmed, time, description);
        }
        /// <summary>
        /// 复制工单时创建逻辑
        /// </summary>
        /// <returns></returns>
        protected abstract OrderBaseEntity CopyCreate();
        /// <summary>
        /// 复制工单
        /// </summary>
        /// <param name="time">复制操作的时间</param>
        /// <param name="status">复制后的工单希望处于什么状态，默认为待确认</param>
        /// <param name="description">此操作的原因</param>
        /// <returns></returns>
        public virtual OrderBaseEntity Copy(DateTimeOffset time, Status status = Status.ToBeConfirmed, string description = "复制")
        {
            var entity = CopyCreate();
            if (entity.Status != status)
                entity.ChangeStatus(status, time, description, EmployeeId).ConfigureAwait(false).GetAwaiter().GetResult();
            return entity;
        }
    }
    /// <summary>
    /// 普通/默认工单
    /// </summary>
    public class OrderEntity : OrderBaseEntity, IExtendableObject
    {
        public string ExtensionData { get; set; }
        public string ExtendedField1 { get; set; }
        public string ExtendedField2 { get; set; }
        public string ExtendedField3 { get; set; }
        public string ExtendedField4 { get; set; }
        public string ExtendedField5 { get; set; }

        protected OrderEntity() : base() { }
        protected internal OrderEntity(DateTimeOffset time,
                                       long categoryId,
                                       string title,
                                       string description = default,
                                       UrgencyDegree urgencyDegree = UrgencyDegree.Normalize,
                                       string employeeId = default,
                                       DateTimeOffset? estimatedExecutionTime = default,
                                       DateTimeOffset? estimatedCompletionTime = default,
                                       string extendedField1 = default,
                                       string extendedField2 = default,
                                       string extendedField3 = default,
                                       string extendedField4 = default,
                                       string extendedField5 = default) : base(time,
                                                                               categoryId,
                                                                               title,
                                                                               description,
                                                                               urgencyDegree,
                                                                               employeeId,
                                                                               estimatedExecutionTime,
                                                                               estimatedCompletionTime)
        {
            ExtendedField1 = extendedField1;
            ExtendedField2 = extendedField2;
            ExtendedField3 = extendedField3;
            ExtendedField4 = extendedField4;
            ExtendedField5 = extendedField5;
        }

        protected override OrderBaseEntity CopyCreate()
        {
            return new OrderEntity(StatusChangedTime,
                                   CategoryId,
                                   Title,
                                   Description,
                                   UrgencyDegree,
                                   EmployeeId,
                                   EstimatedExecutionTime,
                                   EstimatedCompletionTime,
                                   ExtendedField1,
                                   ExtendedField2,
                                   ExtendedField3,
                                   ExtendedField4,
                                   ExtendedField5);
        }
    }

    /// <summary>
    /// 实体类是按流程来的，这里提供一些更便捷的方法，批量执行一些步骤
    /// </summary>
    public static class WorkOrderBaseExt
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="time">复制时间</param>
        /// <param name="status">希望复制的工单的初始状态</param>
        /// <param name="description">原因</param>
        /// <returns></returns>
        public static T Copy<T>(this T entity,
                                DateTime? time = default,
                                Status status = Status.ToBeConfirmed,
                                string description = "复制") where T : OrderBaseEntity
        {
            return entity.Copy(time ?? Clock.Now, status, description) as T;
        }

        /// <summary>
        /// 分配或领取工单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time">分配或领取时间，若为空则调用Clock.Now</param>
        /// <param name="employeeId">员工id，若为空则表示此工单不分配给任何员工，只做状态改变</param>
        /// <param name="estimatedExecutionTime">预计开始时间</param>
        /// <param name="estimatedCompletionTime">预计结束时间</param>
        public static void Allocate(this OrderBaseEntity entity,
                                    DateTimeOffset? time = default,
                                    string employeeId = default,
                                    DateTimeOffset? estimatedExecutionTime = default,
                                    DateTimeOffset? estimatedCompletionTime = default)
        {
            entity.Allocate(time ?? Clock.Now, employeeId, estimatedExecutionTime, estimatedCompletionTime);
        }

        /// <summary>
        /// 回退到指定状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time"></param>
        /// <param name="status">目标状态，不指定则默认回到上一个状态。若最终的目标状态已经到顶了，则抛出UserFriendlyException异常</param>
        /// <param name="description">回退原因，如：希望重新分配，可空</param>
        /// <param name="toBeConfirmed">进入待确认状态时的回调函数</param>
        /// <param name="toBeAllocated">进入已确认，待分配状态时的回调函数</param>
        /// <param name="toBeProcessed">进入已分配，待执行状态时的回调函数</param>
        /// <param name="processing">进入已执行，待完成状态时的回调函数</param>
        /// <returns></returns>
        public static async Task BackOff(this OrderBaseEntity entity,
                                         DateTimeOffset? time = default,
                                         Status? status = default,
                                         string description = "回退",
                                         Func<OrderBaseEntity, Task> toBeConfirmed = default,
                                         Func<OrderBaseEntity, Task> toBeAllocated = default,
                                         Func<OrderBaseEntity, Task> toBeProcessed = default,
                                         Func<OrderBaseEntity, Task> processing = default)
        {
            if (status == default)
            {
                if (entity.Status > Status.ToBeConfirmed && entity.Status <= Status.Completed)
                    status = entity.Status - 1;
                else
                    status = entity.Status;
            }

            if (status >= entity.Status)
                throw new UserFriendlyException("workorderBackOffException1".BXJGWorkOrderL(entity.Status.BXJGWorkOrderEnum()));

            var t = time ?? Clock.Now;

            if (entity.Status == Status.Rejected)
            {
                //已拒绝的工单只允许回退到待确认状态
                if (status != Status.ToBeConfirmed)
                    throw new UserFriendlyException("workorderBackOffException1".BXJGWorkOrderL(status.Value.BXJGWorkOrderEnum()));
                else
                {
                    if (toBeConfirmed != null)
                        await toBeConfirmed(entity);
                    entity.UnReject(t, description);
                    //this.SkipRetain(toBeAllocated:toBeAllocated, toBeProcessed:toBeProcessed, processing:processing)
                    return;
                }
            }

            if (status < Status.Completed && entity.Status == Status.Completed)
            {
                if (processing != null)
                    await processing(entity);
                entity.UnCompletion(t, description);
            }

            if (status < Status.Processing && entity.Status == Status.Processing)
            {
                if (toBeProcessed != null)
                    await toBeProcessed(entity);
                entity.UnExecute(t, description);
            }

            if (status < Status.ToBeProcessed && entity.Status == Status.ToBeProcessed)
            {
                if (toBeAllocated != null)
                    await toBeAllocated(entity);
                entity.UnAllocate(t, description);
            }

            if (status < Status.ToBeAllocated && entity.Status == Status.ToBeAllocated)
            {
                if (toBeConfirmed != null)
                    await toBeConfirmed(entity);
                entity.UnConfirme(t, description);
            }
        }

        /// <summary>
        /// 跳跃到指定状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="time">执行时间，若为空则使用Clock.Now</param>
        /// <param name="status">目标状态，若为空则跳跃到下一个状态</param>
        /// <param name="description">改变状态的说明</param>
        /// <param name="empId">若目标状态大于等于分配，则需要指明分配操作相关参数</param>
        /// <param name="estimatedExecutionTime">预计开始时间</param>
        /// <param name="estimatedCompletionTime">预计结束时间</param>
        /// <param name="excuteTime">实际开始时间，若为空则保留之前的值，若还为空则使用time的值，若还未空则使用Clock.Now</param>
        /// <param name="completeTime">实际结束时间，若为空则保留之前的值，若还为空则使用time的值，若还未空则使用Clock.Now</param>
        /// <param name="toBeAllocated">进入已确认，待分配状态时的回调函数</param>
        /// <param name="toBeProcessed">进入已分配，待执行状态时的回调函数</param>
        /// <param name="processing">进入已执行，待完成状态时的回调函数</param>
        /// <param name="completed">进入已完成状态时的回调函数</param>
        /// <param name="rejected">进入拒绝状态时的回调函数</param>
        public static async Task Skip(this OrderBaseEntity entity,
                                      DateTimeOffset? time = default,
                                      Status? status = default,
                                      string description = default,
                                      string empId = default,
                                      DateTimeOffset? estimatedExecutionTime = default,
                                      DateTimeOffset? estimatedCompletionTime = default,
                                      DateTimeOffset? excuteTime = default,
                                      DateTimeOffset? completeTime = default,
                                      Func<OrderBaseEntity, Task> toBeAllocated = default,
                                      Func<OrderBaseEntity, Task> toBeProcessed = default,
                                      Func<OrderBaseEntity, Task> processing = default,
                                      Func<OrderBaseEntity, Task> completed = default,
                                      Func<OrderBaseEntity, Task> rejected = default)
        {
            if (status == default)
            {
                if (entity.Status >= Status.ToBeConfirmed && entity.Status < Status.Completed)
                    status = entity.Status + 1;
                else
                    status = entity.Status;
            }

            if (status <= entity.Status)
                throw new UserFriendlyException("workorderSkipException1".BXJGWorkOrderL(entity.Status.BXJGWorkOrderEnum()));

            DateTimeOffset t = time ?? Clock.Now;

            if (status == Status.Rejected)
            {
                if (rejected != null)
                    await rejected(entity);
                entity.Reject(t, description);
                return;
            }

            if (status > Status.ToBeConfirmed && entity.Status == Status.ToBeConfirmed)
            {
                if (toBeAllocated != null)
                    await toBeAllocated(entity);
                entity.Confirme(t, description);
            }

            if (status > Status.ToBeAllocated && entity.Status == Status.ToBeAllocated)
            {
                if (toBeProcessed != null)
                    await toBeProcessed(entity);
                entity.Allocate(t, empId, estimatedExecutionTime, estimatedCompletionTime, description);
            }

            if (status > Status.ToBeProcessed && entity.Status == Status.ToBeProcessed)
            {
                if (processing != null)
                    await processing(entity);
                entity.Execute(excuteTime ?? t, description);
            }

            if (status > Status.Processing && entity.Status == Status.Processing)
            {
                if (completed != null)
                    await completed(entity);
                entity.Completion(completeTime ?? t, description);
            }
        }

        /// <summary>
        /// 设置为任意状态。这是个魔法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="status">目标状态</param>
        /// <param name="time">回退时间，为空则使用Clock.Now</param>
        /// <param name="description">改变状态的说明</param>
        /// <param name="empId">若目标状态大于等于分配，则需要指明分配操作相关参数</param>
        /// <param name="estimatedExecutionTime">预计开始时间</param>
        /// <param name="estimatedCompletionTime">预计结束时间</param>
        /// <param name="excuteTime">实际开始时间</param>
        /// <param name="completeTime">实际结束时间</param>
        /// <param name="toBeConfirmed">进入待确认状态时的回调函数</param>
        /// <param name="toBeAllocated">进入已确认，待分配状态时的回调函数</param>
        /// <param name="toBeProcessed">进入已分配，待执行状态时的回调函数</param>
        /// <param name="processing">进入已执行，待完成状态时的回调函数</param>
        /// <param name="completed">进入已完成状态时的回调函数</param>
        /// <param name="rejected">进入拒绝状态时的回调函数</param>
        public static async Task ChangeStatus(this OrderBaseEntity entity,
                                              Status status,
                                              DateTimeOffset? time = default,
                                              string description = default,
                                              string empId = default,
                                              DateTimeOffset? estimatedExecutionTime = default,
                                              DateTimeOffset? estimatedCompletionTime = default,
                                              DateTimeOffset? excuteTime = default,
                                              DateTimeOffset? completeTime = default,
                                              Func<OrderBaseEntity, Task> toBeConfirmed = default,
                                              Func<OrderBaseEntity, Task> toBeAllocated = default,
                                              Func<OrderBaseEntity, Task> toBeProcessed = default,
                                              Func<OrderBaseEntity, Task> processing = default,
                                              Func<OrderBaseEntity, Task> completed = default,
                                              Func<OrderBaseEntity, Task> rejected = default)
        {
            if (status < entity.Status)
                await entity.BackOff(time,
                                     status,
                                     description,
                                     toBeConfirmed,
                                     toBeAllocated,
                                     toBeProcessed,
                                     processing);
            else if (status > entity.Status)
                await entity.Skip(time,
                                  status,
                                  description,
                                  empId,
                                  estimatedExecutionTime,
                                  estimatedCompletionTime,
                                  excuteTime,
                                  completeTime,
                                  toBeAllocated,
                                  toBeProcessed,
                                  processing,
                                  completed,
                                  rejected);
            else
            {
                //由于此方法有其它参数，若不报异常，调用方会认为其它参数处理成功了
                throw new UserFriendlyException("workorderChangeStatusException1".BXJGWorkOrderL());
            }
        }
    }
}