using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Utils.Share.DataPermission
{
    public record class DataPermissionDto
    {
        /// <summary>
        /// 针对指定实体类型的在各组织单位中数据的数据权限规则
        /// </summary>
        public ICollection<DataPermissionOrganizationUnitDto> OrganizationUnits { get; set; }
        /// <summary>
        /// 针对指定实体类型总的数据权限规则
        /// </summary>
        public DataPermissionGrantType GrantType { get; set; }
    }
    /// <summary>
    /// 针对指定组织单位的数据权限规则
    /// </summary>
    public record class DataPermissionOrganizationUnitDto
    {
        public long? OrganizationUnitId { get; set; }
        public string? OrganizationUnitCode { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public DataPermissionGrantType GrantType { get; set; }
    }
}
