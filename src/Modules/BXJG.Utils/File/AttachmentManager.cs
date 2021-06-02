using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Linq;
using Abp.Domain.Entities;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 一个简单的附件管理器
    /// </summary>
    public class AttachmentManager : BXJGUtilsAppServiceBase
    {
        private readonly TempFileManager tempFileManager;
        private readonly IRepository<EntityFileEntity, Guid> repository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        private readonly string entityType;

        public AttachmentManager(TempFileManager tempFileManager, IRepository<EntityFileEntity, Guid> repository, string entityType)
        {
            this.tempFileManager = tempFileManager;
            this.repository = repository;
            this.entityType = entityType;
        }
        /// <summary>
        /// 设置附件
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="files">key文件相对路径，value扩展属性</param>
        /// <returns></returns>
        public async Task<List<EntityFileEntity>> SetAttachmentsAsync(object entityId, IDictionary<string, IDictionary<string, object>> files)
        {
            //删除原来的
            await repository.DeleteAsync(c => c.EntityId == entityId.ToString());

            //添加新的
            var list = new List<EntityFileEntity>();
            foreach (var item in files)
            {
                var slt = tempFileManager.ConvertToThumPath(item.Key);
                //var ss = tempFileManager.Relative2AbsolutePath
                var entity = new EntityFileEntity
                {
                    EntityType = entityType,
                    EntityId = entityId.ToString(),
                    FileUrl = item.Key,
                    ThumUrl = tempFileManager.HasThumImage(item.Key) ? tempFileManager.ConvertToThumPath(item.Key) : default
                };
                foreach (var ext in item.Value)
                {
                    entity.SetData(ext.Key, ext.Value);
                }
                list.Add(entity);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return list;
        }
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public Task<List<EntityFileEntity>> GetAttachmentsAsync(params object[] entityIds)
        {
            var ids = entityIds.Select(c => c.ToString());
            var q = repository.GetAll().Where(c => ids.Contains(c.EntityId));
            return AsyncQueryableExecuter.ToListAsync(q);
        }
    }
    /// <summary>
    /// 一个简单的附件管理器
    /// </summary>
    /// <typeparam name="TEntity">附件所属实体类型</typeparam>
    public class AttachmentManager<TEntity> : AttachmentManager
    {
        public AttachmentManager(TempFileManager tempFileManager, IRepository<EntityFileEntity, Guid> repository)
            : base(tempFileManager, repository, typeof(TEntity).FullName)
        {
        }
    }
}
