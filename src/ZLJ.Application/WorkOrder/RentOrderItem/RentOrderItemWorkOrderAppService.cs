using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BXJG.Common.Dto;
using BXJG.WorkOrder.WorkOrder;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Authorization;
using workOrderCls = BXJG.WorkOrder.WorkOrderCategory;
using Abp.Linq.Extensions;
using BXJG.WorkOrder.WorkOrderCategory;
using BXJG.Utils.File;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using System;
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using System.Collections.Generic;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using Abp.UI;
using Abp.Extensions;
using ZLJ.WorkOrder;

namespace ZLJ.App.Admin.WorkOrder.RentOrderItem.Admin
{
    /// <summary>
    /// 后台管理（关联租赁单明细的）工单，也就是设备维修工单
    /// </summary>
    public class RentOrderItemWorkOrderAppService : WorkOrderAppServiceBase<CreateInput,
                                                                            UpdateInput,
                                                                            BatchOperationInputLong,
                                                                            BatchOperationOutputLong,
                                                                            EntityDto<long>,
                                                                            GetTotalInput,
                                                                            GetAllInputBase<GetTotalInput>,
                                                                            Dto,
                                                                            BatchChangeStatusInputBase,
                                                                            BatchOperationOutputLong,
                                                                            BatchConfirmeInput,
                                                                            BatchOperationOutputLong,
                                                                            BatchAllocateInputBase,
                                                                            BatchOperationOutputLong,
                                                                            RentOrderItemWorkOrderEntity,
                                                                            IRepository<RentOrderItemWorkOrderEntity, long>,
                                                                            RentOrderItemWorkOrderCreateDto,
                                                                            RentOrderItemWorkOrderManager,
                                                                            IRepository<workOrderCls.CategoryEntity, long>,
                                                                            QueryTemp<RentOrderItemWorkOrderEntity>>

    {
        private readonly Lazy<IRepository<StaffInfoEntity, long>> staffRepository;
        public RentOrderItemWorkOrderAppService(IRepository<RentOrderItemWorkOrderEntity, long> repository,
                                            Lazy<RentOrderItemWorkOrderManager> manager,
                                            Lazy<IRepository<CategoryEntity, long>> categoryRepository,
                                            Lazy<AttachmentManager<RentOrderItemWorkOrderEntity>> attachmentManager,
                                            Lazy<IRepository<CategoryEntity, long>> clsRepository,
                                            Lazy<IRepository<StaffInfoEntity, long>> staffRepository,
                                            Lazy<CategoryManager> clsManager) : base(repository,
                                                                                     manager,
                                                                                     categoryRepository,
                                                                                     attachmentManager,
                                                                                     clsRepository,
                                                                                     clsManager,
                                                                                     WorkOrderConsts.RentOrderItemWorkOrder,
                                                                                     PermissionNames.WorkOrderManagerCreate,
                                                                                     PermissionNames.WorkOrderManagerUpdate,
                                                                                     PermissionNames.WorkOrderManagerDelete,
                                                                                     PermissionNames.WorkOrderManager,
                                                                                     PermissionNames.WorkOrderConfirme,
                                                                                     PermissionNames.WorkOrderAllocate,
                                                                                     PermissionNames.WorkOrderExecute,
                                                                                     PermissionNames.WorkOrderCompletion,
                                                                                     PermissionNames.WorkOrderReject)
        {
            this.staffRepository = staffRepository;
        }
        //public override async Task<Dto> CreateAsync(CreateInput input)
        //{
        //    try
        //    {
        //        return await base.CreateAsync(input);
        //    }
        //    catch (RentOrderItemWorkOrderOnlyException ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}
        //public override async Task<Dto> UpdateAsync(UpdateInput input)
        //{
        //    try
        //    {
        //        return await base.UpdateAsync(input);
        //    }
        //    catch (RentOrderItemWorkOrderOnlyException ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}
        protected override async ValueTask<RentOrderItemWorkOrderCreateDto> CreateInputToCreateDto(CreateInput input)
        {
            var t = await base.CreateInputToCreateDto(input);
            t.RentOrderItemId = input.RentOrderItemId;
            return t;
        }
        protected override async ValueTask<object> GetStateAsync(IEnumerable<QueryTemp<RentOrderItemWorkOrderEntity>> entities)
        {
            //if (entities == null || entities.Count() == 0)
            //    return null;

            var empIds = entities.Select(c => c.Order.EmployeeId);

            //var query = from c in staffRepository.Value.GetAll().AsNoTrackingWithIdentityResolution()
            //            join d in userRepository.Value.GetAll().AsNoTrackingWithIdentityResolution() on c.UserId equals d.Id into g
            //            from e in g.DefaultIfEmpty()
            //            where 
            //            select Tuple.Create(c, e);

            var emps = await staffRepository.Value.GetAll()
                                                  .AsNoTrackingWithIdentityResolution()
                                                  .Include(c => c.Roles)
                                                  .Where(c => empIds.Contains(c.Id.ToString()))
                                                  .ToListAsync();
            return emps;
        }
        protected override Dto EntityToDto(QueryTemp<RentOrderItemWorkOrderEntity> temp, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.EntityToDto(temp, images, state);
            IEnumerable<StaffInfoEntity> staffs = state as IEnumerable<StaffInfoEntity>;
            var staff = staffs.SingleOrDefault(c => c.Id.ToString() == dto.EmployeeId);
            if (staff != null)
            {
                dto.EmployeeName = staff.Name;
                dto.EmployeePhone = staff.PhoneNumber;
            }
            return dto;
        }
        protected override IQueryable<RentOrderItemWorkOrderEntity> GetOrderQuery()
        {
            var query = base.GetOrderQuery();
            query = query.Include(c => c.RentOrderItem).ThenInclude(c => c.Order).ThenInclude(c => c.AssociatedCompany);
            query = query.Include(c => c.RentOrderItem).ThenInclude(c => c.EquipmentInstance).ThenInclude(c => c.EquipmentInfo).ThenInclude(c => c.Brand);
            return query;
        }
        protected override async ValueTask<Expression<Func<QueryTemp<RentOrderItemWorkOrderEntity>, bool>>> ApplyKeyword(string keyword)
        {
            Expression<Func<QueryTemp<RentOrderItemWorkOrderEntity>, bool>> expression = await base.ApplyKeyword(keyword);

            expression = expression.Or(c => c.Order.RentOrderItem.Order.AssociatedCompany.Name.Contains(keyword))
                                   .Or(c => c.Order.RentOrderItem.Order.AssociatedCompany.LinkMan.Contains(keyword))
                                   .Or(c => c.Order.RentOrderItem.Order.AssociatedCompany.Address.Contains(keyword))
                                   .Or(c => c.Order.RentOrderItem.EquipmentInstance.EquipmentInfo.Model.Contains(keyword));
            return expression;
        }
        protected override async ValueTask<IQueryable<QueryTemp<RentOrderItemWorkOrderEntity>>> ApplyOther(IQueryable<QueryTemp<RentOrderItemWorkOrderEntity>> query, GetTotalInput input)
        {
            query = await base.ApplyOther(query, input);
            query = query.WhereIf(input.CustomerId.HasValue, c => input.CustomerId.Value == c.Order.RentOrderItem.Order.AssociatedCompanyId)
                         .WhereIf(input.RentOrderId.HasValue, c => input.RentOrderId.Value == c.Order.RentOrderItem.OrderId)
                         .WhereIf(input.RentOrderItemId.HasValue, c => input.RentOrderItemId.Value == c.Order.RentOrderItemId)
                         .WhereIf(!input.EquipmentId.IsNullOrWhiteSpace(), c => input.EquipmentId == c.Order.RentOrderItem.EquipmentInstanceId);
            return query;
        }
        /// <summary>
        /// 尝试获取指定设备的工单
        /// 一个租赁明细，也就是一个客户那里的设备，只能允许有一个未完成且未拒绝的工单，此方法尝试获取这个工单
        /// 注：新增或调整工单状态时会做重复性检查，参考<see cref="ZLJ.WorkOrder.RentOrderItemWorkOrder.EventHandler"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Dto> GetUnfinishedAsync(GetExistInput input)
        {
            await base.CheckGetPermissionAsync();
            var ids = await base.manager.Value.GetUnfinishedAsync(input.RentOrderItemId);
            var id = ids.FirstOrDefault();
            if (id != default)
            {
                return await base.GetAsync(new EntityDto<long>(id));
            }
            return null;
            //var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
            //var q = GetQuery().Where(c => c.Order.RentOrderItemId == input.RentOrderItemId && !pc.Contains(c.Order.Status));
            //var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(q);
            //return await EntityToDto(entity);
        }

        //protected override ValueTask ConfirmeSingleAsync(RentOrderItemWorkOrderEntity entity, BatchConfirmeInput input)
        //{
        //  return  this.manager.Value.ConfirmeAsync(entity, input.StatusChangedTime, "确认", input.Points);
        //    //return base.ConfirmeSingleAsync(entity, input);
        //}
    }
}