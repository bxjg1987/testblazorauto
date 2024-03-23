
using Abp.Domain.Entities;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BXJG.Utils.Files
{
    /*
     * 附件下载器 跟 文件下载器 分开定义，以便实施不同权限策略
     * 附件权限是跟实体关联的，单独的文件管理则直接通过功能权限来控制。
     * 
     * 所以权限跟附件关联
     * 
     * 而多个报销单，单独设置权限比较浪费，所以 权限只与实体类型关联即可。
     * 
     * 下载和删除 就不用分开了。
     * 
     * 权限变更时，这里的记录也得变
     * 
     * 
     */

    /// <summary>
    /// 附件权限
    /// </summary>
    public class AttachmentPermissionEntity : Entity<Guid> //: BaseEntityGuid
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        //没必要定义实体属性名，因为不需要那么精细

        /// <summary>
        /// 实体id
        /// </summary>
        public string? EntityId { get; set; }
        ///// <summary>
        ///// 文件id
        ///// </summary>
        //public Guid? FileId { get; set; }
        ///// <summary>
        ///// 文件
        ///// </summary>
        ////[ForeignKey(nameof(Id))]
        ////[Required]
        //public virtual FileEntity? File { get; set; }

        /// <summary>
        /// 允许下载的权限名称
        /// </summary>
        public string? DownloadPermissionName { get; set; }
        /// <summary>
        /// 允许删除的权限名称
        /// </summary>
        public string? DeletePermissionName { get; set; }
    }
}