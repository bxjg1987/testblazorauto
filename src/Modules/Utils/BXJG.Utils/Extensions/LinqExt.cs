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
        /// <param name="entityType">实体类型，若实体id是全局唯一的，比如guid，则此参数可省略</param>
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns>附件查询结果</returns>
        public static IQueryable<AttachmentEntity> WhereAttachment(this IQueryable<AttachmentEntity> q, string? entityType=default, string? propertyName = default, bool track = false, params string[] entityIds)
        {
            q = q.Include(x => x.File)
                 .WhereIf(entityType.IsNotNullOrWhiteSpaceBXJG(), x => x.EntityType == entityType)
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
        /// 应用附件查询条件，泛型版
        /// </summary>
        /// <param name="q"></param>
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns>附件查询结果</returns>
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
        /// <param name="entityType">实体类型，若实体id是全局唯一的，比如guid，则此参数可省略</param>
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="track"></param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where(this IQueryable<TagEntity> q, string? entityType = default, string propertyName = default, bool track = false)
        {
            q = q.WhereIf(entityType.IsNotNullOrWhiteSpaceBXJG(), x => x.EntityType == entityType)
                 .WhereIf(propertyName.IsNotNullOrWhiteSpaceBXJG(), x => x.PropertyName == propertyName);

            if (!track)
                q = q.AsNoTrackingWithIdentityResolution();


            return q;
        }
        /// <summary>
        /// 应用Tag查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="track"></param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where(this IQueryable<TagEntity> q, string? entityType=default, string propertyName = default, bool track = false, params string[] entityIds)
        {
            q = q.Where(entityType,propertyName,track);

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
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="track"></param>
        /// <param name="entityIds">实体id列表</param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where<TEntity>(this IQueryable<TagEntity> q, string propertyName = default, bool track = false, params string[] entityIds)
        {
            return q.Where(typeof(TEntity).FullName, propertyName, track, entityIds);
        }
        /// <summary>
        /// 应用Tag查询条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="propertyName">属性名称，若不提供则忽略此条件，若你想获取属性为空的，你需要后续继续where加条件</param>
        /// <param name="track"></param>
        /// <returns></returns>
        public static IQueryable<TagEntity> Where<TEntity>(this IQueryable<TagEntity> q, string propertyName = default, bool track = false)
        {
            return q.Where(typeof(TEntity).FullName, propertyName, track);
        }
        #endregion


        #region abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        private static Func<object, CancellationToken, Task<int>> _batchDeleteImpl;
        [Obsolete]
        public static Func<object, CancellationToken, Task<int>> BatchDeleteImpl
        {
            get => _batchDeleteImpl;
            set => _batchDeleteImpl = value;
        }
        /// <summary>
        /// abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete]
        public static Task<int> BatchDelete<T>(this IQueryable<T> q, CancellationToken cancellationToken = default)
        {
            if (_batchDeleteImpl == null)
                throw new InvalidOperationException("BatchDelete未初始化，请确保EFCoreModule已加载");
            return _batchDeleteImpl(q, cancellationToken);
        }
        #endregion
    }
}
