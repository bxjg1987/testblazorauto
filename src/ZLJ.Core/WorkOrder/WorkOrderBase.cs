using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Timing;
using Abp.UI;
using BXJG.Utils.Extensions;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.WorkOrder
{
    public class PointsChangingEventData : EntityChangingEventData<WorkOrderBaseEntity>
    {
        public int? Original { get; private set; }
        public PointsChangingEventData(WorkOrderBaseEntity entity, int? original) : base(entity)
        {
            Original = original;
        }
    }
    public abstract class WorkOrderBaseEntity : OrderBaseEntity
    {
        public WorkOrderBaseEntity()
        {
        }

        public WorkOrderBaseEntity(DateTimeOffset time, long categoryId, string title, string description = null, UrgencyDegree urgencyDegree = UrgencyDegree.Normalize, string employeeId = null, DateTimeOffset? estimatedExecutionTime = null, DateTimeOffset? estimatedCompletionTime = null) : base(time, categoryId, title, description, urgencyDegree, employeeId, estimatedExecutionTime, estimatedCompletionTime)
        {
        }

        bool pointscsh = false;
        int? points1, jfysz;
        object lockerPoints = new object();

        /// <summary>
        /// 工单对应的积分（注:如果配置为单量模式,积分固定为1分，如果配置为积分模式，则确定工单时，必须设置工单对应积分值）
        /// </summary>
        public virtual int? Points
        {
            get { return points1; }
            set
            {
                if (Status >= Status.Completed)
                    throw new UserFriendlyException("已完成或拒绝的工单调整积分没有意义！".L());

                if (!pointscsh)
                {
                    lock (lockerPoints)
                    {
                        if (!pointscsh)
                        {
                            jfysz = value;
                            pointscsh = true;
                        }
                    }
                }
                //确保事件与状态同步
                lock (lockerPoints)
                {
                    points1 = value;
                    if (value != jfysz)
                        DomainEvents.AddOrReplace(new PointsChangingEventData(this, jfysz.Value));
                    else
                        DomainEvents.Remove<PointsChangingEventData>();
                }
            }
        }

        public override void Confirme(DateTimeOffset time, string description = "确认")
        {
            DefaultPoints();
            base.Confirme(time, description);
        }

        public override void UnExecute(DateTimeOffset time, string description = "反执行")
        {
            DefaultPoints();
            base.UnExecute(time, description);
        }
        void DefaultPoints()
        {
            //if (!Points.HasValue)
            //    throw new UserFriendlyException("确认前请确保已设置积分！".L());
            if (Points == default)
                Points = 1;
        }
    }

    //public abstract class WorkOrderBaseManager<TEntity> : OrderBaseManager<TEntity> where TEntity : WorkOrderBaseEntity
    //{
    //    //IRepository<ZLJ.Rent.Order.OrderItemEntity, long> rentOrderItemRepository;
    //    //IRepository<ZLJ.Rent.Order.OrderEntity, long> rentOrderRepository;

    //    public WorkOrderBaseManager(IRepository<TEntity, long> repository,
    //                                IRepository<CategoryEntity, long> clsRepository,
    //                                CategoryManager clsManager,
    //                                OrderNoGenerator orderNoGenerator) : base(repository,
    //                                                                          clsRepository,
    //                                                                          clsManager,
    //                                                                          WorkOrderConsts.RentOrderItemWorkOrder,
    //                                                                          orderNoGenerator)
    //    { }

    //    public ValueTask Confirme(TEntity entity, DateTimeOffset? time, string desc = "确认")
    //    {
    //        //这里可以从配置中获取默认的工单与积分的比例值
    //        entity.Confirme(time ?? Clock.Now, desc);
    //        return ValueTask.CompletedTask;
    //    }
    //}
}
