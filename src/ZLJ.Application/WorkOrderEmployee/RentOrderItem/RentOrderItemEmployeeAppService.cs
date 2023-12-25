
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BXJG.Common.Dto;
using BXJG.Utils.File;
using BXJG.WorkOrder.EmployeeApplication.WorkOrder;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.WorkOrderCategory;
using BXJG.WorkOrder.WorkOrderType;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Authorization;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ZLJ.BaseInfo.StaffInfo;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using BXJG.WorkOrder.Session;
using ZLJ.Authorization.Users;
using ZLJ.WorkOrder;

namespace ZLJ.WorkOrderEmployee.RentOrderItem
{
    public class RentOrderItemEmployeeAppService : BXJG.WorkOrder.EmployeeApplication.WorkOrder.WorkOrderAppServiceBase<EntityDto<long>,
                                                                                                                        GetAllInput,
                                                                                                                        GetTotalInput,
                                                                                                                        RentOrderItemDto,
                                                                                                                        BXJG.WorkOrder.EmployeeApplication.WorkOrder.BatchAllocateInputBase,
                                                                                                                        BatchOperationOutputLong,
                                                                                                                        BXJG.WorkOrder.EmployeeApplication.WorkOrder.BatchChangeStatusInputBase,
                                                                                                                        BatchOperationOutputLong,
                                                                                                                        BXJG.WorkOrder.EmployeeApplication.WorkOrder.BatchChangeStatusInputBase,
                                                                                                                        BatchOperationOutputLong,
                                                                                                                        BXJG.WorkOrder.EmployeeApplication.WorkOrder.BatchChangeStatusInputBase,
                                                                                                                        BatchOperationOutputLong,
                                                                                                                        RentOrderItemWorkOrderEntity,
                                                                                                                        IRepository<RentOrderItemWorkOrderEntity, long>,
                                                                                                                        RentOrderItemWorkOrderManager,
                                                                                                                        QueryTemp<RentOrderItemWorkOrderEntity>>
    {
        private readonly Lazy<IRepository<StaffInfoEntity, long>> staffRepository;
        public RentOrderItemEmployeeAppService(IRepository<RentOrderItemWorkOrderEntity, long> repository,
                                               IEmployeeSession empSession,
                                               Lazy<RentOrderItemWorkOrderManager> manager,
                                               Lazy<IRepository<CategoryEntity, long>> categoryRepository,
                                               Lazy<AttachmentManager<RentOrderItemWorkOrderEntity>> attachmentManager,
                                               Lazy<CategoryManager> clsManager,
                                               Lazy<IRepository<StaffInfoEntity, long>> staffRepository,
                                               WorkOrderTypeManager workOrderTypeManager) : base(repository,
                                                                                                 empSession,
                                                                                                 manager,
                                                                                                 categoryRepository,
                                                                                                 attachmentManager,
                                                                                                 clsManager,
                                                                                                 workOrderTypeManager,
                                                                                                 WorkOrderConsts.RentOrderItemWorkOrder,
                                                                                                 PermissionNames.EmployeeAppGdKhsb,
                                                                                                 PermissionNames.EmployeeAppGdKhsblq,
                                                                                                 PermissionNames.EmployeeAppGdKhsbzx,
                                                                                                 PermissionNames.EmployeeAppGdKhsbwc,
                                                                                                 PermissionNames.EmployeeAppGdKhsbjj)
        {
            this.staffRepository = staffRepository;
        }

        protected override async ValueTask<IQueryable<QueryTemp<RentOrderItemWorkOrderEntity>>> ApplyOther(IQueryable<QueryTemp<RentOrderItemWorkOrderEntity>> query, GetTotalInput input)
        {
            query = await base.ApplyOther(query, input);
            query = query.WhereIf(input.CustomerId.HasValue, c => input.CustomerId.Value == c.Order.RentOrderItem.Order.AssociatedCompanyId)
                         .WhereIf(input.RentOrderId.HasValue, c => input.RentOrderId.Value == c.Order.RentOrderItem.OrderId)
                         .WhereIf(input.RentOrderItemId.HasValue, c => input.RentOrderItemId.Value == c.Order.RentOrderItemId)
                         .WhereIf(input.EquipmentId.HasValue, c => input.EquipmentId.Value == c.Order.RentOrderItem.EquipmentInstanceId);
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
        protected override IQueryable<RentOrderItemWorkOrderEntity> GetOrderQuery()
        {
            var query = base.GetOrderQuery();
            query = query.Include(c => c.RentOrderItem).ThenInclude(c => c.Order).ThenInclude(c => c.AssociatedCompany);
            query = query.Include(c => c.RentOrderItem).ThenInclude(c => c.EquipmentInstance).ThenInclude(c => c.EquipmentInfo).ThenInclude(c => c.Brand);
            return query;
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
                                                  .Include(c => c.User)
                                                  .Where(c => empIds.Contains(c.Id.ToString()))
                                                  .ToListAsync();
            return emps;
        }
        protected override RentOrderItemDto EntityToDto(QueryTemp<RentOrderItemWorkOrderEntity> temp, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.EntityToDto(temp, images, state);
            IEnumerable<StaffInfoEntity> staffs = state as IEnumerable<StaffInfoEntity>;
            var staff = staffs.SingleOrDefault(c => c.Id.ToString() == dto.EmployeeId);
            if (staff != null)
            {
                dto.EmployeeName = staff.Name;
                dto.EmployeePhone = staff.User.PhoneNumber;
            }
            return dto;
        }
    }
}