using Abp.Domain.Repositories;
using BXJG.Utils.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.Extensions
{
    public static class RepositoryExt
    {
        #region 附件

        //这里只定义常用场景需要的查询，更多场景不要定义扩展，而是直接在需要时自己查询

        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>key属性名，value文件列表</returns>
        public static async Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, string entityType, string entityId, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, track: track).Where(x => x.EntityId == entityId);
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.OrderBy(c => c.OrderIndex).Select(c => c.File).ToList());
        }

        /// <summary>
        /// 从附件中获取文件，忽略propertyName
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>
        public static async Task<List<FileEntity>> GetFilesByAttachmentWithoutProperty(this IRepository<AttachmentEntity, Guid> repository, string entityType, string entityId, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, track: track).Where(x => x.EntityId == entityId);
            return await q.OrderBy(x => x.OrderIndex).Select(x => x.File).ToListAsync(cancellationToken);
        }


        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：属性名和文件列表</returns>
        public static async Task<Dictionary<string, Dictionary<string, List<FileEntity>>>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, string entityType, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, track: track, entityIds: entityIds.ToArray());
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.EntityId)
                       .ToDictionary(x => x.Key,
                                     x => x.GroupBy(y => y.PropertyName)
                                           .ToDictionary(z => z.Key,
                                                         z => z.OrderBy(v => v.OrderIndex).Select(g => g.File).ToList()));
        }
        /// <summary>
        /// 从附件中获取文件，忽略propertyName
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：文件列表</returns>
        public static async Task< Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty(this IRepository<AttachmentEntity, Guid> repository, string entityType, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, track: track, entityIds: entityIds.ToArray());
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.EntityId).ToDictionary(x => x.Key,  z => z.OrderBy(v => v.OrderIndex).Select(g => g.File).ToList());
        }





        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体id</param>
        /// <param name="propertyName">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>
        public static async Task<List<FileEntity>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, string entityType, string entityId, string propertyName=default, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, propertyName, track).Where(x => x.EntityId == entityId).OrderBy(x => x.OrderIndex);
            var list = await q.ToArrayAsync(cancellationToken);
            return list.Select(x => x.File).ToList();
        }



        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>key属性名，value文件列表</returns>
        public static Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(typeof(TEntity).FullName, entityId, track, cancellationToken);
        }
        /// <summary>
        /// 从附件中获取文件，忽略propertyName
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>
        public static Task<List<FileEntity>> GetFilesByAttachmentWithoutProperty<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachmentWithoutProperty(typeof(TEntity).FullName, entityId, track, cancellationToken);
        }

        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：属性名和文件列表</returns>
        public static Task<Dictionary<string, Dictionary<string, List<FileEntity>>>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(typeof(TEntity).FullName, entityIds, track, cancellationToken);
        }

        /// <summary>
        /// 从附件中获取文件，忽略propertyName
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：文件列表</returns>
        public static  Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty<TEntity>(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachmentWithoutProperty(typeof(TEntity).FullName, entityIds, track, cancellationToken);
        }


        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="propertyName">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>
        public static Task<List<FileEntity>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, string propertyName=default, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(typeof(TEntity).FullName, entityId, propertyName, track, cancellationToken);
        }



        #endregion}
    }
}