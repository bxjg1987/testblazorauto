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
using System.IO;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Abp;
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using Abp.BackgroundJobs;
using Microsoft.Extensions.Logging;
using BXJG.Utils.Extensions;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 附件管理器
    /// 每个实体类型对应一个附件管理器
    /// 它与实体建立弱引用(EntityType,EntityId和可选的属性名)关系，将所有实体的附件统一存储到一张表中
    /// 它内部使用FileManager进行文件管理
    /// </summary>
    public class AttachmentManager : BXJGBaseDomainService
    {
        /// <summary>
        /// 文件管理器
        /// </summary>
        public FileManager FileManager { get; set; }
        /// <summary>
        /// 附件仓储
        /// </summary>
        public IRepository<AttachmentEntity, Guid> Repository { get; set; }
        /// <summary>
        /// 关联的实体类型，也就表明当前附件管理器是专用于管理此类型实体的
        /// </summary>
        protected readonly string entityType;
        public IGuidGenerator GuidGenerator { get; set; }
        /// <summary>
        /// 实例化附件管理器
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public AttachmentManager(string entityType)
        {
            this.entityType = entityType;
        }
        /// <summary>
        /// 设置附件，删除和新增关联的文件
        /// </summary>
        /// <param name="entityId">实体id</param>
        /// <param name="files">包含新老文件的列表，注意顺序，若是纯删除则保持空</param>
        /// <param name="propertyName">关联的属性名</param>
        /// <returns>没必要返回附件，直接返回文件吧</returns>
        public async Task<List<FileEntity>> SetAttachments(object entityId, string propertyName, string propertyDisplayName=default, IList<SetAttachmentFile> files = default ,FilePermission filePermission= FilePermission.Authenticated)
        {
            var id = entityId.ToString();

            var oldEntities = await (await Repository.GetAllAsync())
                                              .WhereAttachment(entityType, propertyName, true, entityId.ToString())
                                              .ToArrayAsync(CancellationTokenProvider.Token);

            if (files == default)
                files = new List<SetAttachmentFile>();

            var needDeletes = oldEntities.Where(x => !files.Any(d => d.FileId == x.FileId)).ToImmutableArray();
            foreach (var item in needDeletes)
            {
                await Repository.DeleteAsync(item);
                //await FileManager.Remove(item.File);
            }

            var newEntities = new List<FileEntity>();
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var entity = oldEntities.SingleOrDefault(x => x.FileId == file.FileId);
                if (entity == default)
                {
                    Logger.Warn(""+System.Text.Json.JsonSerializer.Serialize(file));
                    var f = await FileManager.Upload(file.FileName, file.TempPath,filePermission);
                    entity = new AttachmentEntity
                    {
                        EntityId = id,
                        EntityType = entityType,
                        Id = GuidGenerator.Create(),// f.Id,
                        File = f,
                        FileId = f.Id,
                        OrderIndex = i,
                        TenantId = f.TenantId,//不晓得为啥非要在这里设置下，估计应该用IMustHaveTenant接口
                        PropertyName = propertyName,
                        PropertyDisplayName= propertyDisplayName?? propertyName,
                    };
                    await Repository.InsertAsync(entity);
                }
                else
                {
                    entity.OrderIndex = i;
                }
                newEntities.Add(entity.File);
            }

            return newEntities;
        }

        //获取以仓储扩展方法提供


        ///// <summary>
        ///// 获取附件
        ///// </summary>
        ///// <param name="entityIds"></param>
        ///// <param name="propertyName">关联的属性名</param>
        ///// <returns>key为实体id，value为附件列表</returns>
        //public async Task<Dictionary<string, List<AttachmentEntity>>> GetAttachmentsAsync(string propertyName = default, params string[] entityIds)
        //{
        //    entityIds = entityIds.Distinct().ToArray();
        //    var q = Repository.GetAll()
        //                      .Where(c => c.EntityType == entityType && entityIds.Contains(c.EntityId))
        //                      .Where(c => c.PropertyName == propertyName)
        //                      .OrderBy(c => c.OrderIndex);
        //    var list = await q.ToListAsync();
        //    list.ForEach(entity =>
        //    {
        //        entity.AbsoluteFileUrl = FileManager.Relative2AbsoluteUrl(entity.RelativeFileUrl);
        //        entity.RelativeThumUrl = FileManager.TryGetThumRelativeUrl(entity.AbsoluteFileUrl);
        //        if (!entity.RelativeThumUrl.IsNullOrWhiteSpace())
        //            entity.AbsoluteThumUrl = FileManager.Relative2AbsoluteUrl(entity.RelativeThumUrl);
        //    });
        //    var dic = new Dictionary<string, List<AttachmentEntity>>();

        //    foreach (var item in entityIds)
        //    {
        //        var sss = list.Where(c => c.EntityId == item).ToList();
        //        if (sss.Count > 0)
        //            dic.Add(item, sss);
        //    }
        //    return dic;
        //}

        ///// <summary>
        ///// 获取附件
        ///// </summary>
        ///// <param name="entityIds"></param>
        ///// <returns>key为实体id，value第一个附件</returns>
        //public async Task<Dictionary<string, AttachmentEntity>> GetFirstAttachmentsAsync(params string[] entityIds)
        //{
        //    //先用low的方式实现功能吧
        //    var items = await GetAttachmentsAsync(entityIds: entityIds);
        //    foreach (var item in items)
        //    {
        //        while (item.Value.Count > 1)
        //        {
        //            item.Value.RemoveAt(item.Value.Count - 1);
        //        }
        //    }
        //    return items.ToDictionary(c => c.Key, c => c.Value.FirstOrDefault());
        //}
    }
    /// <summary>
    /// 通用泛型附件管理器
    /// </summary>
    /// <typeparam name="TEntity">附件所属实体类型</typeparam>
    public class AttachmentManager<TEntity> : AttachmentManager
    {
        public AttachmentManager() : base(typeof(TEntity).FullName)
        {
        }
    }
}
