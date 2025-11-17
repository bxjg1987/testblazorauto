using Abp.Application.Services.Dto;
using Abp.Auditing;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZLJ.Core;

namespace ZLJ.Application.Share.StaffInfo;

public class StaffInfoEditDto : ZLJ.Application.Common.Share.User.UserEditDto// EntityDto<long>
{
    #region 基本信息
    /// <summary>
    /// 性别
    /// </summary>
    [Display(Name = "性别")]
    public Gender Gender { get; set; }
    /// <summary>
    /// 生日
    /// </summary>
    [Display(Name = "出生日期")]
    public DateTimeOffset? Birthday { get; set; }

    /// <summary>
    /// 身份证
    /// </summary>
    [StringLength(Core.Share.ZLJConsts.StaffInfoIdNumberMaxLength)]
    [Display(Name = "身份证号")]
    public string? IdNumber { get; set; }
    /// <summary>
    /// 入职日期
    /// </summary>
   [Display(Name = "入职日期")]
    public DateTimeOffset? InDate { get; set; }
    /// <summary>
    /// 离职日期
    /// </summary>
     [Display(Name = "离职日期")]
    public DateTimeOffset? OutDate { get; set; }
    ///// <summary>
    ///// 员工编号
    ///// 暂时不要
    ///// </summary>
    //[StringLength(ZLJ.Core.Share.ZLJConsts.StaffInfoNoMaxLength)]
    //public string No { get; set; }
    /// <summary>
    /// 所属区域Id
    /// </summary>
    [Display(Name = "所属区域")]
    public long? AreaId { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    [StringLength(Core.Share.ZLJConsts.StaffInfoCurrentAddressMaxLength)]
    [Display(Name = "地址")]
    public string? CurrentAddress { get; set; }
    #endregion


    //public long[] ouIds { get; set; }
}
