using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Linq;
using Abp.Localization.Sources;
using BXJG.Utils.OperationLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils.Localization;
using BXJG.WorkOrder.WorkOrderCategory;
using Abp.Domain.Uow;
using Abp.Extensions;

namespace BXJG.WorkOrder.WorkOrder
{
    public abstract class OperationLogAppService<TDto,
                                                 TPropertyDto,
                                                 TGetAllInput,
                                                 TUser,
                                                 TEntitySet> : Utils.OperationLog.OperationLogAppService<TDto,
                                                                                                         TPropertyDto,
                                                                                                         TGetAllInput,
                                                                                                         TUser,
                                                                                                         TEntitySet> where TDto : Dto<TPropertyDto>
                                                                                                                     where TPropertyDto : PropertyDto
                                                                                                                     where TGetAllInput : GetAllInput
                                                                                                                     where TUser : AbpUserBase
                                                                                                                     where TEntitySet : EntitySet
    {
        protected readonly IRepository<CategoryEntity, long> clsRepository;
        /// <summary>
        /// 工单模块中的本地化源
        /// </summary>
        protected readonly Lazy<ILocalizationSource> workOrderLocalizationSource;
        //protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        protected readonly string clsPropertyName = nameof(WorkOrder.OrderBaseEntity.CategoryId);
        protected readonly string urgencyDegreePropertyName = nameof(WorkOrder.OrderBaseEntity.UrgencyDegree);
        protected readonly string statusPropertyName = nameof(WorkOrder.OrderBaseEntity.Status);
        //protected readonly string empPropertyName = nameof(WorkOrder.OrderBaseEntity.EmployeeId);

        public OperationLogAppService(IRepository<EntityChange, long> repository,
                                      IRepository<EntityChangeSet, long> setRepository,
                                      IRepository<TUser, long> userRepository,
                                      IRepository<CategoryEntity, long> clsRepository,
                                      string permissionName = null) : base(repository,
                                                                           setRepository,
                                                                           userRepository,
                                                                           permissionName)
        {
            workOrderLocalizationSource = new Lazy<ILocalizationSource>(() => LocalizationManager.GetSource(CoreConsts.LocalizationSourceName));
            this.clsRepository = clsRepository;
        }

        protected override async Task BeforeMapAsync(IList<EntitySet> entityChanges)
        {
            var t1 = base.BeforeMapAsync(entityChanges);
            var clsIds = new HashSet<string>();
            foreach (var item in entityChanges)
            {
                var item2 = item.Entity.PropertyChanges.SingleOrDefault(d => d.PropertyName == clsPropertyName);
                if (item2 != null)
                {
                    if (!clsIds.Contains(item2.NewValue))
                        clsIds.Add(item2.NewValue);
                    if (!clsIds.Contains(item2.OriginalValue))
                        clsIds.Add(item2.OriginalValue);
                }
            }
            Task<List<NameValueDto>> t2 = Task.FromResult(new List<NameValueDto>());
            if (clsIds.Count > 0)
            {
                var query = clsRepository.GetAll().Where(c => clsIds.Contains(c.Id.ToString())).Select(c => new NameValueDto { Name = c.Id.ToString(), Value = c.DisplayName });
                t2 = AsyncQueryableExecuter.ToListAsync(query);
            }
            await Task.WhenAll(t1, t2);
            CurrentUnitOfWork.Items["cls"] = t2.Result;
        }
        protected override ValueTask ForEachPropertiesAsync(TDto dto, TPropertyDto property)
        {
            property.PropertyDisplayName = this.workOrderLocalizationSource.Value.GetStringOrNull(CoreConsts.WorkOrder + "." + property.PropertyName);
            if (property.PropertyName == clsPropertyName)
            {
                var cls = CurrentUnitOfWork.Items["cls"] as List<NameValueDto>;
                //if (!property.OriginalValue.IsNullOrWhiteSpace())
                property.OriginalValueDisplayName = cls.SingleOrDefault(c => c.Name == property.OriginalValue)?.Value;
                property.NewValueDisplayName = cls.SingleOrDefault(c => c.Name == property.NewValue)?.Value;
            }
            if (property.PropertyName == urgencyDegreePropertyName)
            {
                if (!property.OriginalValue.IsNullOrWhiteSpace())
                    property.OriginalValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.OriginalValue).BXJGWorkOrderEnum();
                property.NewValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.NewValue).BXJGWorkOrderEnum();
            }
            if (property.PropertyName == statusPropertyName)
            {
                if (!property.OriginalValue.IsNullOrWhiteSpace())
                    property.OriginalValueDisplayName = Enum.Parse<WorkOrder.Status>(property.OriginalValue).BXJGWorkOrderEnum();
                property.NewValueDisplayName = Enum.Parse<WorkOrder.Status>(property.NewValue).BXJGWorkOrderEnum();
            }
            return ValueTask.CompletedTask;
        }
    }

    public abstract class OperationLogAppService<TUser> : OperationLogAppService<Dto<PropertyDto>,
                                                                                 PropertyDto,
                                                                                 GetAllInput,
                                                                                 TUser,
                                                                                 EntitySet> where TUser : AbpUserBase
    {
        protected OperationLogAppService(IRepository<EntityChange, long> repository,
                                         IRepository<EntityChangeSet, long> setRepository,
                                         IRepository<TUser, long> userRepository,
                                         IRepository<CategoryEntity, long> clsRepository,
                                         string permissionName = null) : base(repository,
                                                                              setRepository,
                                                                              userRepository,
                                                                              clsRepository,
                                                                              permissionName)
        {
        }
    }
}
