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

namespace BXJG.WorkOrder.OperationLog
{
    public class OperationLogAppService<TDto,
                                        TPropertyDto,
                                        TGetAllInput,
                                        TUser,
                                        TEntitySet> : BXJG.Utils.OperationLog.OperationLogAppService<TDto,
                                                                                                     TPropertyDto,
                                                                                                     TGetAllInput,
                                                                                                     TUser,
                                                                                                     TEntitySet>
       where TDto : Dto<TPropertyDto>
       where TPropertyDto : PropertyDto
       where TGetAllInput : GetAllInput
       where TUser : AbpUserBase
       where TEntitySet : EntitySet
    {
        protected readonly IRepository<WorkOrderCategory.CategoryEntity, long> clsRepository;
        protected readonly ILocalizationSource localizationSource;
        //protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        protected readonly string clsPropertyName = nameof(WorkOrder.OrderBaseEntity.CategoryId);
        protected readonly string urgencyDegreePropertyName = nameof(WorkOrder.OrderBaseEntity.UrgencyDegree);
        protected readonly string statusPropertyName = nameof(WorkOrder.OrderBaseEntity.Status);

        public OperationLogAppService(IRepository<EntityChange, long> repository,
                                      IRepository<EntityChangeSet, long> setRepository,
                                      IRepository<TUser, long> userRepository,
                                      IRepository<WorkOrderCategory.CategoryEntity, long> clsRepository,
                                      string permissionName = null) : base(repository,
                                                                           setRepository,
                                                                           userRepository,
                                                                           permissionName)
        {
            localizationSource = base.LocalizationManager.GetSource(CoreConsts.LocalizationSourceName);
            this.clsRepository = clsRepository;
        }

        protected override async Task BeforeMapAsync(IList<EntitySet> entityChanges)
        {
            await base.BeforeMapAsync(entityChanges);

            var cl = new List<string>();
            foreach (var item in entityChanges)
            {
                var ps = item.Entity.PropertyChanges.Where(d => d.PropertyName == clsPropertyName);
                foreach (var item2 in ps)
                {
                    if (!cl.Contains(item2.NewValue))
                        cl.Add(item2.NewValue);
                    if (!cl.Contains(item2.OriginalValue))
                        cl.Add(item2.OriginalValue);
                }
            }
            if (cl.Count > 0)
            {
                var query = clsRepository.GetAll().Where(c => cl.Contains(c.Id.ToString())).Select(c => new NameValueDto { Name = c.Id.ToString(), Value = c.DisplayName });
                var cls = await AsyncQueryableExecuter.ToListAsync(query);
                CurrentUnitOfWork.Items["cls"] = cls;
            }
        }
        protected override async ValueTask ForEachPropertiesAsync(TDto dto, TPropertyDto property)
        {
            property.PropertyDisplayName = this.localizationSource.GetString(dto.EntityEntityTypeFullName + "." + property.PropertyName);
            if (property.PropertyName == clsPropertyName)
            {
                var cls = CurrentUnitOfWork.Items["cls"] as List<NameValueDto>;
                property.OriginalValueDisplayName = cls.SingleOrDefault(c => c.Name == property.OriginalValue)?.Value;
                property.NewValueDisplayName = cls.SingleOrDefault(c => c.Name == property.NewValue)?.Value;
            }
            if (property.PropertyName == urgencyDegreePropertyName)
            {
                property.OriginalValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.OriginalValue).BXJGWorkOrderEnum();
                property.NewValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.NewValue).BXJGWorkOrderEnum();
            }
            if (property.PropertyName == statusPropertyName)
            {
                property.OriginalValueDisplayName = Enum.Parse<WorkOrder.Status>(property.OriginalValue).BXJGWorkOrderEnum();
                property.NewValueDisplayName = Enum.Parse<WorkOrder.Status>(property.NewValue).BXJGWorkOrderEnum();
            }
        }
    }
}
