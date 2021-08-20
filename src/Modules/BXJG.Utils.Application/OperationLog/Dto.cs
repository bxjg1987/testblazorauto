using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Linq;
using Abp.Authorization;
using System.ComponentModel.DataAnnotations;
using Abp.Linq.Extensions;
using AutoMapper;
using BXJG.Common.Dto;
using Abp.Authorization.Users;
using Abp.Domain.Uow;

namespace BXJG.Utils.OperationLog
{
    /// <summary>
    /// 操作日志dto
    /// </summary>
    public class Dto<TPropertyDto> : IExtendableDto
    {
        /// <summary>
        /// 操作时的浏览器信息
        /// </summary>
        public virtual string SetBrowserInfo { get; set; }
        /// <summary>
        /// 客户端id
        /// </summary>
        public virtual string SetClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string SetClientName { get; set; }
        /// <summary>
        /// 操作员id
        /// </summary>
        public virtual long? SetUserId { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string SetReason { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual dynamic ExtensionData { get; set; }
        /// <summary>
        /// 被操作的实体类型
        /// </summary>
        public virtual string EntityEntityTypeFullName { get; set; }
        /// <summary>
        /// 被操作的实体的id
        /// </summary>
        public virtual string EntityEntityId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTimeOffset EntityChangeTime { get; set; }
        /// <summary>
        /// 被改过的字段
        /// </summary>
        public virtual List<TPropertyDto> EntityPropertyChanges { get; set; }
    }

    /// <summary>
    /// 操作日志字段dto
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string PropertyName { get; set; }
        /// <summary>
        /// 字段显示名
        /// </summary>
        public virtual string PropertyDisplayName { get; set; }
        /// <summary>
        /// 字段类型名
        /// </summary>
        public virtual string PropertyTypeFullName { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public virtual string NewValue { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public virtual string NewValueDisplayName { get; set; }
        /// <summary>
        /// 修改后的值
        /// </summary>
        public virtual string OriginalValue { get; set; }
        /// <summary>
        /// 修改后的值
        /// </summary>
        public virtual string OriginalValueDisplayName { get; set; }
    }

    /// <summary>
    /// 获取操作日志时的输入模型
    /// </summary>
    public class GetAllInput
    {
        /// <summary>
        /// 实体id
        /// </summary>
        //[Required]
        //public string EntityTypeFullName { get; set; }
        [Required]
        public string EntityId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sorting { get; set; }
    }

    /// <summary>
    /// 查询时临时用的
    /// </summary>
    public class EntitySet
    {
        public EntityChange Entity { get; set; }
        public EntityChangeSet Set { get; set; }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(EntitySet), typeof(Dto<>)).ForMember("ExtensionData",c=>c.MapFrom("Set.ExtensionData"))
                                                       .ForMember("UserName", c => c.Ignore());
            CreateMap<EntityPropertyChange, PropertyDto>();
        }
    }

    //public static class AutoMapperExtensions
    //{
    //    public static IMappingExpression<TSource, TDestination> ApplyDefault<TSource, TDestination>(this IMappingExpression<TSource, TDestination> config)
    //        where TSource : EntitySet
    //        where TDestination : Dto
    //    {
    //        //已全局处理扩展属性，参考：BXJGUtilsModule
    //        //config = config.ForMember(c => c.ExtensionData, opt =>opt.ConvertUsing(c => System.Text.Json.JsonSerializer.Deserialize<dynamic>(c.Set.ExtensionData)));


    //        //return config.ForMember(c => c.Text, opt => opt.MapFrom(c => c.DisplayName));

    //        //return config.ForMember(c => c.Text, opt => opt.MapFrom(c => c.DisplayName))
    //        //        .ForMember(c => c.IconCls, opt => opt.Ignore())
    //        //        .ForMember(c => c.Checked, opt => opt.Ignore());
    //        //.ForMember(c => c.State, opt => opt.Ignore())
    //        //.ForMember(c => c.ExtData, opt => opt.Ignore())

    //        return config;
    //    }
    //}

    //目前只是站在实体的角度
    //可以类似的设计站在操作员的角度

    //为什么用TDto, TPropertyDto, TGetAllInput， TEntitySet这些泛型，而不是写死？
    //希望实现方有机会定义自己的子类dto，它们可能在应用层组合更多属性

    public abstract class OperationLogAppService<TDto, TPropertyDto, TGetAllInput, TUser, TEntitySet> : ApplicationService
        where TDto : Dto<TPropertyDto>
        where TPropertyDto : PropertyDto
        where TGetAllInput : GetAllInput
        where TUser : AbpUserBase
        where TEntitySet : EntitySet
    {
        protected readonly IRepository<EntityChange, long> entityRepository;
        protected readonly IRepository<EntityChangeSet, long> setRepository;
        protected readonly IRepository<TUser, long> userRepository;

        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;

        protected string permissionName;

        public OperationLogAppService(IRepository<EntityChange, long> repository,
                                      IRepository<EntityChangeSet, long> setRepository,
                                      IRepository<TUser, long> userRepository,
                                      string permissionName = default)
        {
            this.entityRepository = repository;
            this.setRepository = setRepository;
            this.userRepository = userRepository;
            this.permissionName = permissionName;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public async Task<long> GetTotalAsync(TGetAllInput input)
        {
            await CheckPermissionAsync();
            var query = await CreateFilterAsync(input);
            return await AsyncQueryableExecuter.CountAsync(query);
        }
        [UnitOfWork(false)]
        public async Task<PagedResultDto<TDto>> GetAllAsync(TGetAllInput input)
        {
            await CheckPermissionAsync();
            var query = await CreateFilterAsync(input);
            var totle = await AsyncQueryableExecuter.CountAsync(query);
            query = this.Sort(query, input);
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            await BeforeMapAsync(list);
            var dots = new List<TDto>();
            foreach (var item in list)
            {
                dots.Add(await Map2DtoAsync(item));
            }
            return new PagedResultDto<TDto>(totle, dots);
        }

        protected virtual async ValueTask CheckPermissionAsync()
        {
            if (!string.IsNullOrEmpty(permissionName))
                await PermissionChecker.AuthorizeAsync(permissionName);
        }

        protected virtual ValueTask<IQueryable<EntitySet>> CreateFilterAsync(TGetAllInput input)
        {
            var query = from c in entityRepository.GetAllIncluding(c => c.PropertyChanges)
                        join d in setRepository.GetAll() on c.EntityChangeSetId equals d.Id into temp
                        from e in temp.DefaultIfEmpty()
                        select new EntitySet
                        {
                            Entity = c,
                            Set = e
                        };

            query = query.Where(c => c.Entity.EntityId == input.EntityId)
                         .WhereIf(input.StartTime.HasValue, c => c.Entity.ChangeTime > input.StartTime)
                         .WhereIf(input.EndTime.HasValue, c => c.Entity.ChangeTime <= input.EndTime);

            return ValueTask.FromResult(query);
        }

        protected virtual IOrderedQueryable<EntitySet> Sort(IQueryable<EntitySet> query, TGetAllInput input)
        {
            IOrderedQueryable<EntitySet> q;
            if (!input.Sorting.IsNullOrWhiteSpace())
                q = query.OrderBy(input.Sorting);
            else
                q = query.OrderByDescending(c => c.Entity.ChangeTime);
            return q;
        }

        protected virtual async Task BeforeMapAsync(IList<EntitySet> entityChanges)
        {
            var userIds = entityChanges.Select(c => c.Set.UserId);

            var users = await AsyncQueryableExecuter.ToListAsync(userRepository.GetAll()
                                                                               .Where(c => userIds.Contains(c.Id))
                                                                               .Select(c => new NameValueDto { Name = c.Id.ToString(), Value = c.Name }));

            base.CurrentUnitOfWork.Items["users"] = users;
        }

        protected virtual async ValueTask<TDto> Map2DtoAsync(EntitySet entityChange)
        {
            var r = base.ObjectMapper.Map<TDto>(entityChange);
            var users = CurrentUnitOfWork.Items["users"] as List<NameValueDto>;
            r.UserName = users.SingleOrDefault(c => c.Name == r.SetUserId.ToString())?.Value;
            //数量少，使用Parallel.For反而性能更低
            foreach (var item in r.EntityPropertyChanges)
            {
                await ForEachPropertiesAsync(r, item);
            }
            return r;
        }

        protected virtual ValueTask ForEachPropertiesAsync(TDto dto, TPropertyDto property)
        {
            property.PropertyDisplayName = base.L(dto.EntityEntityTypeFullName + "." + property.PropertyName);
            return ValueTask.CompletedTask;
        }
    }

    public abstract class OperationLogAppService<TUser> : OperationLogAppService<Dto<PropertyDto>,
                                                                                 PropertyDto,
                                                                                 GetAllInput,
                                                                                 TUser,
                                                                                 EntitySet> where TUser : AbpUserBase
    {
        public OperationLogAppService(IRepository<EntityChange, long> repository,
                                      IRepository<EntityChangeSet, long> setRepository,
                                      IRepository<TUser, long> userRepository, string permissionName = null) : base(repository,
                                                                                                                    setRepository,
                                                                                                                    userRepository,
                                                                                                                    permissionName)
        {
        }
    }

    #region 通用实现
    //public class OptLogDefine
    //{
    //    public string TypeName { get; set; }

    //    public string EntityFullName { get; set; }

    //    public string PermissionName { get; set; }

    //    public Type HandlerType { get; set; }

    //    public IReadOnlyDictionary<string,string> 
    //}
    //public class OptLogDefineManager
    //{ 

    //}
    //public class GetAllInput1 : GetAllInput
    //{
    //    public string TypeName { get; set; }
    //}

    //public class OperationLogAppService<TUser> : OperationLogAppService<Dto,
    //                                                                    PropertyDto,
    //                                                                    GetAllInput1,
    //                                                                    TUser,
    //                                                                    EntitySet>
    //    where TUser : AbpUserBase
    //{
    //    public OperationLogAppService(IRepository<EntityChange, long> repository,
    //                                  IRepository<EntityChangeSet, long> setRepository,
    //                                  IRepository<TUser, long> userRepository,
    //                                  string permissionName = null) : base(repository,
    //                                                                       setRepository,
    //                                                                       userRepository,
    //                                                                       permissionName)
    //    {
    //    }
    //}
    #endregion
}
