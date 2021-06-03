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
using System.Linq.Dynamic.Core;
using BXJG.Common.Extensions;
namespace BXJG.Utils.File
{
    /// <summary>
    /// 一个low版附件管理器
    /// 它与实体建立弱引用(EntityType,EntityId)关系，将所有实体的附件统一存储到一张表中
    /// <see href="https://gitee.com/bxjg1987_admin/abp/wikis/pages?sort_id=4086113%26doc_id=627313" />
    /// </summary>
    public class AttachmentManager : BXJGUtilsAppServiceBase
    {
        private readonly TempFileManager tempFileManager;
        private readonly IRepository<AttachmentEntity, Guid> repository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        private readonly string entityType;

        public AttachmentManager(TempFileManager tempFileManager, IRepository<AttachmentEntity, Guid> repository, string entityType)
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
        public async Task<List<AttachmentEntity>> SetAttachmentsAsync(object entityId, IDictionary<string, IDictionary<string, object>> files)
        {
            //相应的文件处理参考：https://gitee.com/bxjg1987_admin/abp/wikis/pages?sort_id=4086113%26doc_id=627313#%E4%BA%8B%E5%8A%A1
            await repository.DeleteAsync(c => c.EntityId == entityId.ToString());
            //await CurrentUnitOfWork.SaveChangesAsync();

            //添加新的
            var list = new List<AttachmentEntity>();
            foreach (var item in files)
            {
                var entity = new AttachmentEntity
                {
                    EntityType = entityType,
                    EntityId = entityId.ToString(),
                    RelativeFileUrl = tempFileManager.Absolute2RelativeUrl(item.Key),
                };
                entity.AbsoluteFileUrl = tempFileManager.Relative2AbsoluteUrl(entity.RelativeFileUrl);
                entity.RelativeThumUrl = tempFileManager.TryGetThumRelativeUrl(entity.RelativeFileUrl);
                if (!entity.RelativeThumUrl.IsNullOrWhiteSpace())
                    entity.AbsoluteThumUrl = tempFileManager.Relative2AbsoluteUrl(entity.RelativeThumUrl);
                foreach (var ext in item.Value)
                {
                    entity.SetData(ext.Key, ext.Value);
                }
                list.Add(entity);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            //移动所有文件
            await tempFileManager.MoveAsync(list.Select(c => c.RelativeFileUrl).ToArray());

            return list;
        }
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public async Task<Dictionary<object, List<AttachmentEntity>>> GetAttachmentsAsync(params object[] entityIds)
        {
            var ids = entityIds.Select(c => c.ToString());
            var q = repository.GetAll().Where(c => ids.Contains(c.EntityId)).OrderBy(c => c.OrderIndex);
            var list = await AsyncQueryableExecuter.ToListAsync(q);
            list.ForEach(entity =>
            {
                entity.AbsoluteFileUrl = tempFileManager.Relative2AbsoluteUrl(entity.RelativeFileUrl);
                entity.RelativeThumUrl = tempFileManager.TryGetThumRelativeUrl(entity.RelativeFileUrl);
                if (!entity.RelativeThumUrl.IsNullOrWhiteSpace())
                    entity.AbsoluteThumUrl = tempFileManager.Relative2AbsoluteUrl(entity.RelativeThumUrl);
            });
            var dic = new Dictionary<object, List<AttachmentEntity>>();
            foreach (var item in entityIds)
            {
                dic.Add(item, list.Where(c => c.EntityId == item.ToString()).ToList());
            }
            return dic;
        }
    }
    /// <summary>
    /// 一个简单的附件管理器
    /// </summary>
    /// <typeparam name="TEntity">附件所属实体类型</typeparam>
    public class AttachmentManager<TEntity> : AttachmentManager
    {
        public AttachmentManager(TempFileManager tempFileManager, IRepository<AttachmentEntity, Guid> repository)
            : base(tempFileManager, repository, typeof(TEntity).FullName)
        {
        }
    }
}
