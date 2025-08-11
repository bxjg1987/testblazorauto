using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using BXJG.Utils.Files;
using BXJG.Utils.Tag;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.Extensions
{
    /// <summary>
    /// 整个utils core层 对linq相关扩展
    /// </summary>
    public static class LinqExt
    {
        #region 附件
        /// <summary>
        /// 应用附件查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns>key属性名，value文件列表</returns>
        public static IQueryable<AttachmentEntity> WhereAttachment(this IQueryable<AttachmentEntity> q, string entityType, string propertyName = default, bool track = false, params string[] entityIds)
        {
            q = q.Include(x => x.File)
                 .Where(x => x.EntityType == entityType)
                 .WhereIf(propertyName.IsNotNullOrWhiteSpaceBXJG(), x => x.PropertyName == propertyName);

            if (!track)
                q = q.AsNoTrackingWithIdentityResolution();

            if (entityIds.Length == 1)
            {
                var id = entityIds[0];
                q = q.Where(x => x.EntityId == id);
            }
            else
                q = q.Where(x => entityIds.Contains(x.EntityId));

            return q;
        }
        /// <summary>
        /// 应用附件查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns>key属性名，value文件列表</returns>
        public static IQueryable<AttachmentEntity> WhereAttachment<TEntity>(this IQueryable<AttachmentEntity> q, string propertyName = default, bool track = false, params string[] entityIds)
        {
            return q.WhereAttachment( typeof(TEntity).FullName, propertyName, track, entityIds);
        }
        #endregion

        #region Tag
        /// <summary>
        /// 应用Tag查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyName">可选的属性名，随意命名的，跟物理字段无关</param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where(this IQueryable<TagEntity> q, string entityType, string propertyName = default, bool track = false, params string[] entityIds)
        {
            q = q.Where(x => x.EntityType == entityType)
                 .WhereIf(propertyName.IsNotNullOrWhiteSpaceBXJG(), x => x.PropertyName == propertyName);

            if (!track)
                q = q.AsNoTrackingWithIdentityResolution();

            if (entityIds.Length == 1)
            {
                var id = entityIds[0];
                q = q.Where(x => x.EntityId == id);
            }
            else
                q = q.Where( x => entityIds.Contains(x.EntityId));

            return q;
        }
        /// <summary>
        /// 应用Tag查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where<TEntity>(this IQueryable<TagEntity> q, string propertyName = default, bool track = false, params string[] entityIds)
        {
            return q.Where(typeof(TEntity).FullName, propertyName, track, entityIds);
        }
        #endregion


        #region abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// <summary>
        /// abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// </summary>
        public static Func<object, CancellationToken, Task<int>> xx;
        /// <summary>
        /// abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<int> BatchDelete<T>(this IQueryable<T> q, CancellationToken cancellationToken = default)
        {
            return xx(q, cancellationToken);
        }
        #endregion
    }
}
