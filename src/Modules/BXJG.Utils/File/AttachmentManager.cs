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
using System.IO;

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
        /// 设置附件，请在你的业务数据操作最后一个步骤调用此方法，或者将你的带事务的操作放在act中
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="files">key文件相对路径，value扩展属性</param>
        /// <param name="act">移动新文件和删除旧文件之前回调</param>
        /// <returns></returns>
        public async Task<List<AttachmentEntity>> SetAttachmentsAsync(object entityId, IDictionary<string, IDictionary<string, object>> files, Func<List<AttachmentEntity>, ValueTask> act = default)
        {
            //相应的文件处理参考：https://gitee.com/bxjg1987_admin/abp/wikis/pages?sort_id=4086113%26doc_id=627313#%E4%BA%8B%E5%8A%A1
            #region 删除旧文件
            //查询旧文件的相对url
            var qq = repository.GetAll().Where(c => c.EntityId == entityId.ToString()).Select(c => c.RelativeFileUrl);
            var qqList = await AsyncQueryableExecuter.ToListAsync(qq);
            //旧文件的名称集合
            var oldNames = qqList.Select(c => Path.GetFileName(c));

            //新文件的名称集合
            if (files == null)
                files = new Dictionary<string, IDictionary<string, object>>();
            var newNames = files.Keys.Select(c => Path.GetFileName(c));

            //找到需要删除的文件名，不含路径，注意文件名是guid，所以可用这样做
            var xj = oldNames.Except(newNames);

            //得到需要删除的文件集合，包含路径
            var needDelete = qqList.Where(c => xj.Any(d => c.Contains(d)));

            //删除旧文件的数据库记录
            await repository.DeleteAsync(c => c.EntityId == entityId.ToString());

            //不要在这里删，要在数据库操作之后删除文件
            //await tempFileManager.RemoveAsync(needDelete.ToArray());

            await CurrentUnitOfWork.SaveChangesAsync();
            #endregion

            #region 添加新的
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
            #endregion

            #region 回调
            if (act != default)
                await act(list);
            #endregion

            #region 最后移动新文件，删除旧文件
            //移动所有文件
            await tempFileManager.MoveAsync(files.Select(c => c.Key).ToArray());
            //删除旧文件
            await tempFileManager.RemoveAsync(needDelete.ToArray());
            #endregion

            return list;
        }
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>key为实体id，value为附件列表</returns>
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
