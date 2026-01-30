using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Utils.Share.DataPermission
{
    public record class DataPermissionDto
    {
        //#region 数据
        ///// <summary>
        ///// 指定的实体类型才会做数据权限控制
        ///// </summary>
        //public string EntityTypeFullName { get; set; }
        /////// <summary>
        /////// MetaData表元数据
        /////// </summary>
        ////public long MetaDataId { get; set; }
        //#endregion

        #region 授权
        /// <summary>
        /// 属于此单位的数据
        /// </summary>
        public IEnumerable<long> OrganizationUnitIds { get; set; }

        public IEnumerable<string> OrganizationUnitCodes { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public DataPermissionGrantType GrantType { get; set; }
        #endregion
    }
}
