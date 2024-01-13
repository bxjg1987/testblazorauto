using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityHistory;
using Abp.Linq;
using Abp.Linq.Extensions;
using AutoMapper;
using BXJG.Utils.Application.Share.OperationLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;
using BXJG.Common.Dto;

namespace BXJG.Utils.Application.OperationLog
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(EntitySet), typeof(Dto<>)).ForMember("ExtensionData", c => c.MapFrom("Set.ExtensionData"))
                                                       .ForMember("EntityTypeDisplayName", c => c.Ignore())
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
            r.EntityTypeDisplayName = base.L(r.EntityEntityTypeFullName);
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
