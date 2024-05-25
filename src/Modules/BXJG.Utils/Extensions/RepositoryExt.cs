using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using BXJG.Common.Contracts;
using BXJG.Utils.Files;
using BXJG.Utils.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
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
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, default, track, entityId);
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
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, default, track, entityId);
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
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, default, track, entityIds.ToArray());
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
        public static async Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty(this IRepository<AttachmentEntity, Guid> repository, string entityType, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, default, track, entityIds.ToArray());
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.EntityId).ToDictionary(x => x.Key, z => z.OrderBy(v => v.OrderIndex).Select(g => g.File).ToList());
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
        public static async Task<List<FileEntity>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, string entityType, string entityId, string propertyName = default, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = repository.GetAll().WhereAttachment(entityType, propertyName, track, entityId).OrderBy(x => x.OrderIndex);
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
        public static Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty<TEntity>(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
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
        public static Task<List<FileEntity>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, string propertyName = default, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(typeof(TEntity).FullName, entityId, propertyName, track, cancellationToken);
        }



        #endregion

        #region 树
        /// <summary>
        /// 根据code获取所有后代节点，并以平铺结构的集合返回
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetFlattenOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, string code = null) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var query = repository.GetAll();
            if (!code.IsNullOrWhiteSpace())
                query = query.Where(c => c.Code.StartsWith(code));
            query = query.OrderBy(c => c.Code);
            return await query.ToListAsync();// AsyncQueryableExecuter.ToListAsync(query);
        }

        //public static string GetParentCode(this string code)
        //{
        //    if (code.Length == Share.BXJGUtilsConsts.CodeUnitLength)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return code.Substring(0, code.Length - Share.BXJGUtilsConsts.CodeUnitLength).TrimEnd('.');
        //    }
        //}

        /// <summary>
        /// 获取指定节点的兄弟节点、父节点(可能为空)
        /// 均包含其后代节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Item1父节点，Item2兄弟节点</returns>
        public static async Task<Tuple<TEntity, List<TEntity>>> GetBrotherWithOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, TEntity entity) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var parentCode = entity.Code.GetParentCode();

            TEntity parent = null;
            List<TEntity> children;

            if (!Abp.Extensions.StringExtensions.IsNullOrWhiteSpace(parentCode))
            {
                children = await repository.GetFlattenOffspringAsync<TEntity>(parentCode);
                parent = children[0];
            }
            else
            {
                //本来也可以用上面的StartsWith，但是直接getAll性能更好
                children = await repository.GetFlattenOffspringAsync();
            }
            children = children.Where(c => c.ParentId.Equals(entity.ParentId)).ToList();
            return new Tuple<TEntity, List<TEntity>>(parent, children);
        }

        #endregion

        #region 杂项
        /// <summary>
        /// 判断指定条件的数据是否已存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> IsExists<TEntity>(this IRepository<TEntity> q, Expression<Func<TEntity, bool>> w, CancellationToken cancellationToken = default) where TEntity:class,IEntity
        {
            return q.GetAll().AnyAsync(w, cancellationToken);
        }
        /// <summary>
        /// 若指定条件的数据已存在，则抛出异常
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <param name="msg"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task IsExistsThrow<TEntity>(this IRepository<TEntity> q, Expression<Func<TEntity, bool>> w, string msg = default, CancellationToken cancellationToken=default) where TEntity : class, IEntity
        {
            if (await q.GetAll().AnyAsync(w, cancellationToken))
            {
                if (msg.IsNullOrWhiteSpace())
                    msg = BXJGUtilsLocalizationExt.UtilsL("数据已存在！");
                throw new UserFriendlyException(msg);
            }
        }
        #endregion
    }
}