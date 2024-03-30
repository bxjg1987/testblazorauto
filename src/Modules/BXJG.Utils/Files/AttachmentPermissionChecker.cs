using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{
    //放弃，请参考文档中的说明

    /// <summary>
    /// 附件权限管理器
    /// </summary>
    public class AttachmentPermissionChecker : BXJGBaseDomainService
    {
        //权限关系存储在settings中

        ///// <summary>
        ///// 仓储
        ///// </summary>
        //public IRepository<AttachmentPermissionEntity, Guid> Repository { get; set; }
        /// <summary>
        /// 关联的实体类型，也就表明当前管理器是专用于管理此类型实体的
        /// </summary>
        protected readonly string entityType;
        /// <summary>
        /// 附件权限管理器
        /// </summary>
        /// <param name="entityType">实体类型</param>
        public AttachmentPermissionChecker(string entityType)
        {
            this.entityType = entityType;
        }

        public async ValueTask<bool> Check() { 
        
        }
    }
}
