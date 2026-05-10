using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using BXJG.Common.Dynamics;
using BXJG.Utils.Application.Share.Tag;
using BXJG.Utils.Extensions;
using BXJG.Utils.Share.Tag;
using BXJG.Utils.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Web.Controllers
{
    /*
     * 都是简单的查询，没必要走应用服务
     */

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : AbpController
    {
        public BXJGUtilsModuleConfig BXJGUtilsModuleConfig { get; set; }
        //public IRepository<TagEntity, Guid> Repository { get; set; }
        public IPermissionDependencyContext PermissionDependencyContext { get; set; }
        public IScopedIocResolver ScopedIocResolver { get; set; }
        public Lazy<IRepository<TagEntity, Guid>> TagRepository { get; set; }

        #region 获取可选tag
        /// <summary>
        /// 获取指定实体的单个属性的可选的tag，按热度排序
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="propertyName">若此实体只有一个属性有tag，则可空</param>
        /// <param name="top">最多获取热度最高的前多数个</param>
        /// <param name="loadFromDb">实体tag中存在的自定义，且热度高的tag也可以作为可选tag吗？</param>
        /// <returns></returns>
        /// <exception cref="AbpAuthorizationException"></exception>
        [HttpGet]
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<List<SelectableTagDto>> GetPropertySelectable([Required] string entityType, string? propertyName = default, int top = 20, bool loadFromDb = true)
        {
            //必须先加个点，与模块模块配置中设置的tag可选源方式一致，且加个点避免冲突
            var key = $"{entityType}.{propertyName ?? string.Empty}";

            var kxlist = BXJGUtilsModuleConfig.SelectableTagProviders.Where(x => x.Key.StartsWith(key));

            //if (!BXJGUtilsModuleConfig.SelectableTagProviders.TryGetValue(key, out var providers))
            //    return list;

            if (kxlist.Count() == 0)
                return null;
            if (kxlist.Count() > 1)
                throw new UserFriendlyException();


            var list = new List<SelectableTagDto>();
            var provider = kxlist.First().Value;

            if (!await provider.PermissionDependency.IsSatisfiedAsync(PermissionDependencyContext))
                throw new AbpAuthorizationException();


            return await GetSelectable(provider, top, loadFromDb);
            //if (!await providers.permissionDependency.IsSatisfiedAsync(PermissionDependencyContext))
            //    throw new AbpAuthorizationException();

            //var ctx = new SelectableTagContext(ScopedIocResolver, entityType, propertyName, top);
            ////由于不晓得后续提供器获取的数据，同一个tag的排序是否较高，所以每个提供器都获取top条数据
            //foreach (var item in providers.providers)
            //{
            //    list.AddRange(ObjectMapper.Map<List<TagDto>>(await item.Invoke(ctx)));
            //}
            ////若前端提交时指定了，或定义数据源时指定了，需要冲tag表后去数据源
            //if (loadFromDb == true || providers.loadFromDb)
            //{
            //    list.AddRange(ObjectMapper.Map<List<TagDto>>(await TagManager.GetSelectableList(ctx)));
            //}

            //return list.OrderByDescending(x => x.OrderIndex)
            //           .DistinctBy(x => x.TagName)
            //           .Take(top)
            //           .ToList();

        }
        /// <summary>
        /// 一次性获取指定类型实体的多个属性的可选tag列表，按热度排序
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="top"></param>
        /// <param name="loadFromDb"></param>
        /// <returns></returns>
        [HttpGet]
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<List<DynamicPropertyDto<List<SelectableTagDto>>>> GetSelectable([Required] string entityType, int top = 20, bool loadFromDb = true)
        {
            //var q = await Repository.GetAllReadonlyAsync();
            //var key = entityType + (propertyName.IsNotNullOrWhiteSpaceBXJG() ? "." + propertyName : "");

            var kxlist = BXJGUtilsModuleConfig.SelectableTagProviders.Where(x => x.Key.StartsWith(entityType + "."));

            if (!kxlist.Any())
                return null;

            var list = new List<DynamicPropertyDto<List<SelectableTagDto>>>();
            foreach (var item in kxlist)
            {
                var provider = item.Value;
                if (!await provider.PermissionDependency.IsSatisfiedAsync(PermissionDependencyContext))
                    continue;  //throw new AbpAuthorizationException();

                //var list = await GetSelectable(provider, top, loadFromDb);

                list.Add(new DynamicPropertyDto<List<SelectableTagDto>>
                {
                    Value = await GetSelectable(provider, top, loadFromDb),
                    PropertyDisplayName = provider.PropertyDisplayName,
                    PropertyName = provider.PropertyName,
                });
            }

            return list;


        }
        async Task<List<SelectableTagDto>> GetSelectable(BXJGUtilsModuleConfig.SelectableTagProviderEntry provider, int top, bool loadFromDb)
        {
            var list = new List<SelectableTagDto>();
            var ctx = new SelectableTagContext(ScopedIocResolver, provider.EntityType, provider.PropertyName, top);
            //由于不晓得后续提供器获取的数据，同一个tag的排序是否较高，所以每个提供器都获取top条数据
            foreach (var item in provider.Providers)
            {
                list.AddRange(await item.Invoke(ctx));
            }
            //若前端提交时指定了，或定义数据源时指定了，需要冲tag表后去数据源
            if (loadFromDb || provider.LoadFromDb)
            {
                list.AddRange(await TagManager.GetSelectableList(ctx));
            }
            list = list.OrderByDescending(x => x.OrderIndex)
                       .DistinctBy(x => x.TagName)
                       .Take(top)
                       .ToList();
            return list;
        }
        #endregion

        #region 获取指定实体的已选择了的tag列表
        /// <summary>
        /// 获取指定实体的单个属性的已选择了的tag列表
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="entityType">实体类型，若实体id是全局唯一类型，如：guid，则类型可以省略，但提供了的话，性能更好</param>
        /// <param name="propertyName">属性名，若此类型的实体只有一个属性使用tag，则此属性可省略</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork(false)]
        public async Task<List<TagDto>> GetPropertyTags([Required] string entityId, string? entityType = default, string? propertyName = default)
        {
            var q = await TagRepository.Value.GetAllAsync();
            var list = await q.Where(entityType, propertyName, false, entityId)
                              .OrderBy(x => x.OrderIndex)
                              .ToArrayAsync(HttpContext.RequestAborted);

            //根据实体类型和属性名分组，
            var kxlist = list.GroupBy(x => new { x.EntityType, x.PropertyName, x.PropertyDisplayName });
            if (kxlist.Count() > 1)
                throw new UserFriendlyException("匹配到多个属性，请指定propertyName参数");
            if (kxlist.Count() == 0)
                return null;

            var kx = kxlist.First();
            return await GetEntityTags(kx.Key.EntityType, kx.Key.PropertyName, kx.Key.PropertyDisplayName, kx);
        }
        /// <summary>
        /// 获取指定实体的多个属性的已选择了的tag列表
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="entityType">实体类型，若实体id是全局唯一类型，如：guid，则类型可以省略，但提供了的话，性能更好</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork(false)]
        public async Task<List<DynamicPropertyDto<List<TagDto>>>> GetTags([Required] string entityId, string? entityType = default)
        {
            var q = await TagRepository.Value.GetAllAsync();
            var list = await q.Where(entityType, default, false, entityId)
                              .OrderBy(x => x.OrderIndex)
                              .ToArrayAsync(HttpContext.RequestAborted);

            //根据实体类型和属性名分组，
            var kxlist = list.GroupBy(x => new { x.EntityType, x.PropertyName, x.PropertyDisplayName });



            if (kxlist.Count() == 0)
                return null;

            var dpList = new List<DynamicPropertyDto<List<TagDto>>>();
            foreach (var item in kxlist)
            {
                List<TagDto> propertyTags = await GetEntityTags(item.Key.EntityType, item.Key.PropertyName, item.Key.PropertyDisplayName, item);

                dpList.Add(new DynamicPropertyDto<List<TagDto>>
                {
                    PropertyDisplayName = item.Key.PropertyDisplayName,
                    PropertyName = item.Key.PropertyName,
                    Value = propertyTags
                });
            }
            return dpList;
        }

        private async Task<List<TagDto>> GetEntityTags(string entityType, string propertyName, string propertyDisplayName, IEnumerable<TagEntity> tags)
        {
            var key = $"{entityType}.{propertyName ?? string.Empty}";
            if (BXJGUtilsModuleConfig.SelectableTagProviders.TryGetValue(key, out var provider))
            {
                if (!await provider.PermissionDependency.IsSatisfiedAsync(PermissionDependencyContext))
                    throw new AbpAuthorizationException();
            }
            var r = ObjectMapper.Map<List<TagDto>>(tags);
            r.ForEach(x => x.IsSelected = true);
            return r;
        }
        #endregion


        #region 获取指定实体的可选和已选的tag列表
        /// <summary>
        /// 获取指定实体的单个属性的可选和已选的tag列表
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="entityType">实体类型，若实体id是全局唯一类型，如：guid，则类型可以省略，但提供了的话，性能更好</param>
        /// <param name="propertyName">属性名，若此类型的实体只有一个属性使用tag，则此属性可省略</param>
        /// <param name="top">最多获取热度最高的前多数个</param>
        /// <param name="loadFromDb">实体tag中存在的自定义，且热度高的tag也可以作为可选tag吗？</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork(false)]
        public async Task<List<TagDto>> GetPropertyAllTags([Required] string entityId, string? entityType = default, string? propertyName = default, int top = 20, bool loadFromDb = true)
        {
            //已选
            var yx = await GetPropertyTags(entityId, entityType, propertyName);

            if (entityType.IsNullOrWhiteSpaceBXJG())
            {
                if (yx.Any() && yx.GroupBy(d => d.EntityType).Count() == 1)
                    entityType = yx.First().EntityType;
                else
                    throw new UserFriendlyException(L("EntityTypeIsNull"));
            }

            if (propertyName.IsNullOrWhiteSpaceBXJG() && yx.Any())
                propertyName = yx.First().PropertyName;

            //可选
            var kx = await GetPropertySelectable(entityType, propertyName, top, loadFromDb);
            foreach (var x in kx)
            {
                if (!yx.Any(d => d.TagName == x.TagName))
                    yx.Add(TagDto.Map(entityId, entityType, propertyName, x));
            }
            return yx;
        }
        /// <summary>
        /// 获取指定实体的多个属性的可选和已选的tag列表
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="entityType">实体类型，若实体id是全局唯一类型，如：guid，则类型可以省略，但提供了的话，性能更好</param>
        /// <param name="top">最多获取热度最高的前多数个</param>
        /// <param name="loadFromDb">实体tag中存在的自定义，且热度高的tag也可以作为可选tag吗？</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork(false)]
        public async Task<List<DynamicPropertyDto<List<TagDto>>>> GetAllTags([Required] string entityId, string? entityType = default, int top = 20, bool loadFromDb = true)
        {
            //获取当前实体已关联的多个tag属性及其每个属性关联的多个tag
            var yxs = await GetTags(entityId, entityType);
            //若调用方未提供entityType，则尝试从yxs获取entityType
            if (entityType.IsNullOrWhiteSpaceBXJG())
            {
                var f = yxs.FirstOrDefault();
                if (f != default)
                {
                    // 若entityId不是全局唯一的
                    if (f.Value != default && f.Value.Any() && yxs.SelectMany(d => d.Value).GroupBy(d => d.EntityType).Count() == 1)
                        entityType = f.Value.First().EntityType;
                }
            }
            if (entityType.IsNullOrWhiteSpaceBXJG())
                throw new UserFriendlyException("请提供entityType参数");

            var kxs = await GetSelectable(entityType, top, loadFromDb);

            foreach (var kxProperty in kxs)
            {
                var yxProperty = yxs.SingleOrDefault(d => d.PropertyName == kxProperty.PropertyName);
                if (yxProperty != null)
                {
                    var yxTags = yxProperty.Value;
                    foreach (var kxTag in kxProperty.Value)
                    {
                        if (!yxTags.Any(yxTag => kxTag.TagName == yxTag.TagName))
                        {
                            yxTags.Add(TagDto.Map(entityId, entityType, kxProperty.PropertyName, kxTag, kxProperty.PropertyDisplayName));
                        }
                    }
                }
                else
                {
                    yxs.Add(new DynamicPropertyDto<List<TagDto>>
                    {
                        PropertyName = kxProperty.PropertyName,
                        PropertyDisplayName = kxProperty.PropertyDisplayName,
                        OrderIndex = kxProperty.OrderIndex,
                        Value = kxProperty.Value.Select(d => TagDto.Map(entityId, entityType, kxProperty.PropertyName, d, kxProperty.PropertyDisplayName)).ToList()
                    });
                }
            }
            return yxs;
        }
        #endregion
    }
}
