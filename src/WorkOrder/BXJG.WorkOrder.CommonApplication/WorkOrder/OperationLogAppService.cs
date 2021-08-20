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
        protected readonly ILocalizationSource localizationSource;
        //protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        protected readonly string clsPropertyName = nameof(WorkOrder.OrderBaseEntity.CategoryId);
        protected readonly string urgencyDegreePropertyName = nameof(WorkOrder.OrderBaseEntity.UrgencyDegree);
        protected readonly string statusPropertyName = nameof(WorkOrder.OrderBaseEntity.Status);

        public OperationLogAppService(IRepository<EntityChange, long> repository,
                                      IRepository<EntityChangeSet, long> setRepository,
                                      IRepository<TUser, long> userRepository,
                                      IRepository<CategoryEntity, long> clsRepository,
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
            var t1 = base.BeforeMapAsync(entityChanges);

            //var t2 = Task.Run(async () =>
            //{
            //   using (var scope = base.UnitOfWorkManager.Begin(new UnitOfWorkOptions { IsTransactional = false }))
            //   {
            var clsIds = new List<string>();
            foreach (var item in entityChanges)
            {
                var ps = item.Entity.PropertyChanges.Where(d => d.PropertyName == clsPropertyName);
                foreach (var item2 in ps)
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
                // await scope.CompleteAsync();
                t2 = AsyncQueryableExecuter.ToListAsync(query);
                //CurrentUnitOfWork.Items["cls"] = cls;
            }
            // await scope.CompleteAsync();
            //      return null;
            //  }
            //});
            await Task.WhenAll(t1, t2);
            CurrentUnitOfWork.Items["cls"] = t2.Result;
            //var clsIds = new List<string>();
            //foreach (var item in entityChanges)
            //{
            //    var ps = item.Entity.PropertyChanges.Where(d => d.PropertyName == clsPropertyName);
            //    foreach (var item2 in ps)
            //    {
            //        if (!clsIds.Contains(item2.NewValue))
            //            clsIds.Add(item2.NewValue);
            //        if (!clsIds.Contains(item2.OriginalValue))
            //            clsIds.Add(item2.OriginalValue);
            //    }
            //}
            //if (clsIds.Count > 0)
            //{
            //    var query = clsRepository.GetAll().Where(c => clsIds.Contains(c.Id.ToString())).Select(c => new NameValueDto { Name = c.Id.ToString(), Value = c.DisplayName });
            //    var cls = await AsyncQueryableExecuter.ToListAsync(query);
            //    CurrentUnitOfWork.Items["cls"] = cls;
            //}
        }
        protected override ValueTask ForEachPropertiesAsync(TDto dto, TPropertyDto property)
        {
            property.PropertyDisplayName = this.localizationSource.GetString(dto.EntityEntityTypeFullName + "." + property.PropertyName);
            if (property.PropertyName == clsPropertyName)
            {
                var cls = CurrentUnitOfWork.Items["cls"] as List<NameValueDto>;
                if (!property.OriginalValue.IsNullOrEmpty())
                    property.OriginalValueDisplayName = cls.SingleOrDefault(c => c.Name == property.OriginalValue)?.Value;
                property.NewValueDisplayName = cls.SingleOrDefault(c => c.Name == property.NewValue)?.Value;
            }
            if (property.PropertyName == urgencyDegreePropertyName)
            {
                if (!property.OriginalValue.IsNullOrEmpty())
                    property.OriginalValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.OriginalValue).BXJGWorkOrderEnum();
                property.NewValueDisplayName = Enum.Parse<WorkOrder.UrgencyDegree>(property.NewValue).BXJGWorkOrderEnum();
            }
            if (property.PropertyName == statusPropertyName)
            {
                if (!property.OriginalValue.IsNullOrEmpty())
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
