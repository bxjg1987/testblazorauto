using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.WorkOrder.RentOrderItem
{
    /// <summary>
    /// 员工api-客户设备工单-操作日志
    /// </summary>
    public class RentOrderItemEmpployeeWorkOrderOperationLogAppService : OperationLogAppService<User>
    {
        protected readonly string empPropertyName = nameof(OrderBaseEntity.EmployeeId);
        protected readonly IRepository<StaffInfoEntity, long> empRepository;

        public RentOrderItemEmpployeeWorkOrderOperationLogAppService(IRepository<EntityChange, long> repository,
                                                                     IRepository<EntityChangeSet, long> setRepository,
                                                                     IRepository<User, long> userRepository,
                                                                     IRepository<StaffInfoEntity, long> empRepository,
                                                                     IRepository<CategoryEntity, long> clsRepository) : base(repository,
                                                                                                                             setRepository,
                                                                                                                             userRepository,
                                                                                                                             clsRepository,
                                                                                                                             PermissionNames.EmployeeAppGd)
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            this.empRepository = empRepository;
        }

        protected override async Task BeforeMapAsync(IList<BXJG.Utils.OperationLog.EntitySet> entityChanges)
        {
            var t1 = base.BeforeMapAsync(entityChanges);
            var empIds = new HashSet<string>();
            foreach (var item in entityChanges)
            {
                var item2 = item.Entity.PropertyChanges.SingleOrDefault(d => d.PropertyName == empPropertyName);
                if (item2 != null)
                {
                    if (!empIds.Contains(item2.NewValue))
                        empIds.Add(item2.NewValue);
                    if (!empIds.Contains(item2.OriginalValue))
                        empIds.Add(item2.OriginalValue);
                }
            }
            Task<List<NameValueDto>> t2 = Task.FromResult(new List<NameValueDto>());
            if (empIds.Count > 0)
            {
                var query = empRepository.GetAll().Where(c => empIds.Contains(c.Id.ToString())).Select(c => new NameValueDto { Name = c.Id.ToString(), Value = c.Name });
                t2 = AsyncQueryableExecuter.ToListAsync(query);
            }
            await Task.WhenAll(t1, t2);
            CurrentUnitOfWork.Items["emp"] = t2.Result;
        }
        protected override async ValueTask ForEachPropertiesAsync(BXJG.Utils.OperationLog.Dto<BXJG.Utils.OperationLog.PropertyDto> dto, BXJG.Utils.OperationLog.PropertyDto property)
        {
            await base.ForEachPropertiesAsync(dto, property);

            if (property.PropertyDisplayName.IsNullOrWhiteSpace())
                property.PropertyDisplayName = base.LocalizationSource.GetStringOrNull(dto.EntityEntityTypeFullName + "." + property.PropertyName);

            if (property.PropertyName == empPropertyName)
            {
                var emp = CurrentUnitOfWork.Items["emp"] as List<NameValueDto>;
                //if (!property.OriginalValue.IsNullOrWhiteSpace())
                property.OriginalValueDisplayName = emp.SingleOrDefault(c => c.Name == property.OriginalValue)?.Value;
                property.NewValueDisplayName = emp.SingleOrDefault(c => c.Name == property.NewValue)?.Value;
            }
        }
    }
}
