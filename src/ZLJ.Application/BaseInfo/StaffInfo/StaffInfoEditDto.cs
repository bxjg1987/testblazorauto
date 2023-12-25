using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZLJ.App.Common.Users;

namespace ZLJ.App.Admin.BaseInfo.StaffInfo
{
    public class StaffInfoEditDto : EditUserDto// EntityDto<long>
    {
        #region 基本信息
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(ZLJConsts.StaffInfoIdNumberMaxLength)]
        public string IdNumber { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTimeOffset? InDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTimeOffset? OutDate { get; set; }
        ///// <summary>
        ///// 员工编号
        ///// 暂时不要
        ///// </summary>
        //[StringLength(ZLJConsts.StaffInfoNoMaxLength)]
        //public string No { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 现住地址
        /// </summary>
        [StringLength(ZLJConsts.StaffInfoCurrentAddressMaxLength)]
        public string CurrentAddress { get; set; }
        #endregion


        public long[] ouIds { get; set; }
    }
    /// <summary>
    /// 后台管理员工的编辑模型
    /// </summary>
    public class StaffInfoCreateDto : CreateUserDto// EntityDto<long>
    {
        #region 基本信息
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
       [StringLength(ZLJConsts.StaffInfoIdNumberMaxLength)]
        public string IdNumber { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTimeOffset? InDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTimeOffset? OutDate { get; set; }
        ///// <summary>
        ///// 员工编号
        ///// 暂时不要
        ///// </summary>
        //[StringLength(ZLJConsts.StaffInfoNoMaxLength)]
        //public string No { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 现住地址
        /// </summary>
        [StringLength(ZLJConsts.StaffInfoCurrentAddressMaxLength)]
        public string CurrentAddress { get; set; }
          
        #endregion

        public long[] ouIds { get; set; }
    }
}