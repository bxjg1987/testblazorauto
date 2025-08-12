using Abp;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.Utils.Extensions;
using BXJG.Utils.Files;
using BXJG.Utils.Share.Tag;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Tag
{
    public class TagManager : BXJGBaseDomainService
    {
        public IRepository<TagEntity, Guid> Repository { get; set; }
        public IGuidGenerator GuidGenerator { get; set; }
        public IAbpSession AbpSession { get; set; }

        protected readonly string entityType;
        public TagManager(string entityType)
        {
            this.entityType = entityType;
        }
        /// <summary>
        /// 为实体（或它的某个假字段）设置标签
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="tags">标签列表，顺序不是很重要，顺序以传入顺序为准，不以OrderIndex属性为准，</param>
        /// <param name="propertyName">可选的属性名</param>
        /// <returns></returns>
        public async Task<List<TagEntity>> Set(object entityId, IList<TagDto> tags = default, string propertyName = default,string proertyDisplayName = default)
        {
            var id = entityId.ToString();

            var oldEntities = await (await Repository.GetAllAsync())
                                              .Where(entityType, propertyName, true, id)
                                              .ToArrayAsync(CancellationTokenProvider.Token);

            if (tags == default)
                tags = new List<TagDto>();

            var needDeletes = oldEntities.Where(x => !tags.Any(d => d.TagName == x.TagName)).ToImmutableArray();
            foreach (var item in needDeletes)
            {
                await Repository.DeleteAsync(item);
                //await FileManager.Remove(item.File);
            }

            var newEntities = new List<TagEntity>();
            for (int i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];
                var entity = oldEntities.SingleOrDefault(x => x.TagName == tag.TagName);
                if (entity == default)
                {
                    //var f = await FileManager.Upload(file.FileName, file.TempPath);
                    entity = new TagEntity
                    {
                        EntityId = id,
                        EntityType = entityType,
                        Id = GuidGenerator.Create(),// f.Id,
                        TagName = tag.TagName,
                        TagDisplayName = tag.TagDisplayName ?? tag.TagName,
                        OrderIndex = i,
                        TenantId = AbpSession.TenantId,//不晓得为啥非要在这里设置下，估计应该用IMustHaveTenant接口
                        PropertyName = propertyName,
                        PropertyDisplayName= proertyDisplayName ?? tag.TagName,
                        //ExtField1 = tag.ExtField1,
                        //ExtField2 = tag.ExtField2,
                        //扩展json字段用abp提供的方式
                    };
                    await Repository.InsertAsync(entity);
                }
                else
                {
                    entity.OrderIndex = i;
                }
                newEntities.Add(entity);
            }

            return newEntities;
        }

        /// <summary>
        /// 大部分的实体类型+属性对应的tag数据类型都希望 预设可选tag列表 + 数据库已增加的自定义tag
        /// 之后按热度排序
        /// 应在SelectableTagProviders为同一个tag数据类型（实体类型+属性）配置两个委托，预设的可选列表 和 此方法获取的数据库的数据
        /// 
        /// 若某个特殊方法不需要数据库的，就别加这个委托
        /// 
        /// 此外目前没考虑缓存问题
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static async ValueTask<List<TagDto>> GetSelectableList(SelectableTagContext ctx)
        {
            var rep = ctx.ScopedIocResolver.Resolve<IRepository<TagEntity, Guid>>();
            var ct = ctx.ScopedIocResolver.Resolve<ICancellationTokenProvider>();

            var q = await rep.GetAllAsync();
            //重复次数越高的标签热度越高，越应该被选择
            return await q.Where(ctx.EntityType, ctx.PropertyName)
                          .GroupBy(x => new { x.TagName, x.TagDisplayName })
                          .Select(x => new
                          {
                              x.Key.TagName,
                              x.Key.TagDisplayName,
                              OrderIndex = x.Count()
                          })
                          .OrderByDescending(d => d.OrderIndex)
                          .Take(ctx.Top)
                          .Select(x => new TagDto(x.TagName, x.TagDisplayName, x.OrderIndex))
                          .ToListAsync(ct.Token);
        }
    }
    public class TagManager<TEntity> : TagManager
    {
        public TagManager() : base(typeof(TEntity).FullName)
        {
        }
    }
}
