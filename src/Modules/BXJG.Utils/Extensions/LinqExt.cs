using Abp.Linq.Extensions;
using BXJG.Utils.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            q = q.Include(x => x.File);
            if (!track)
                q = q.AsNoTrackingWithIdentityResolution();

            if (entityIds.Length == 1)
            {
                var id = entityIds[0];
                q = q.Where(x => x.EntityId == id);
            }

            return q.Where(x => x.EntityType == entityType)
                    .WhereIf(propertyName.IsNotNullOrWhiteSpaceBXJG(), x => x.PropertyName == propertyName)
                    .WhereIf(entityType.Length > 1, x => entityType.Contains(x.EntityId));
        }
        #endregion
    }
}
