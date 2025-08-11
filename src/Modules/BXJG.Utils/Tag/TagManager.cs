using Abp;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
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
        public async Task<List<TagEntity>> Set(object entityId, IList<TagDto> tags = default, string propertyName = default)
        {
            var id = entityId.ToString();

            var oldEntities = await (await Repository.GetAllAsync())
                                              .Where(entityType, propertyName, true, entityId.ToString())
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

        
    }
    public class TagManager<TEntity> : TagManager
    {
        public TagManager() : base(typeof(TEntity).FullName)
        {
        }
    }
}
