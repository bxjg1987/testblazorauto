using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Utils.Metadata;
using BXJG.Utils.Share;
using BXJG.Utils.Share.DataPermission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BXJG.Utils.DataPermission
{
    /// <summary>
    /// 数据权限
    /// </summary>
    [Index(nameof(UserId))]
    [Index(nameof(RoleId))]
    [Index(nameof(UserOrganizationUnit))]
    [Index(nameof(EntityTypeFullName))]
    [Index(nameof(DataOrganizationUnit))]
    [Comment("数据权限")]
    [Table("BXJGDataPermission")]
    public class DataPermissionEntity : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Comment("租户id")]
        public int? TenantId { get; set; }

        #region 用户范围
        /// <summary>
        /// 对具体用户进行数据授权
        /// </summary>
        [Comment("对具体用户进行数据授权")]
        public long? UserId { get; set; }
        /// <summary>
        /// 对指定角色下的用户进行数据授权
        /// </summary>
        [Comment("仅对这个角色下的用户授权")] 
        public int? RoleId { get; set; }
        /// <summary>
        /// 对指定组织单位下的用户进行数据权限控制
        /// </summary>
        [Comment("仅对这个组织单位下的用户授权，但不包含后代单位")] 
        public long? UserOrganizationUnit { get; set; }
        #endregion

        #region 数据范围
        /// <summary>
        /// 指定的实体类型才会做数据权限控制
        /// </summary>
        [MaxLength(BXJGUtilsConsts.EntityTypeMaxLength)]
        [Unicode(false)]
        [Required]
        [Comment("指定的实体类型才会做数据权限控制")]
        public string EntityTypeFullName { get; set; }
        /// <summary>
        /// MetaData表元数据
        /// </summary>
        [Comment("MetaData表元数据")]
        public long MetaDataId { get; set; }
        public virtual MetadataEntity Metadata { get; set; }
        /// <summary>
        /// 属于此单位的数据
        /// EntityTypeFullName指定数据类型是必须的，在它基础上进一步限定指定单位的数据进行权限控制
        /// </summary>
        [Comment("属于此单位的数据 EntityTypeFullName指定数据类型是必须的，在它基础上进一步限定指定单位的数据进行权限控制")]
        public long? DataOrganizationUnit { get; set; }
        #endregion

        /// <summary>
        /// 授权类型
        /// 没有指定DataOrganizationUnit时：1全部允许 2拒绝 4仅自己
        /// 否则 1指定部门及其后代部门全部数据都允许 2拒绝 4仅自己 8仅访问指定组织单位下的数据
        /// </summary>
        public DataPermissionGrantType GrantType { get; set; }
    }
}