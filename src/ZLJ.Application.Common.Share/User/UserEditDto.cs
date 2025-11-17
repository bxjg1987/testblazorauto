using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.User
{
    //public class UserEditBaseDto
    //{
    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    [StringLength(ZLJ.Core.Share.ZLJConsts.RemarkMaxLength)]
    //    [Display(Name = "备注")]
    //    public string? Remark { get; set; }
    //    /// <summary>
    //    /// 是否关联登录
    //    /// </summary>
    //    [Display(Name = "是否关联登录")]
    //    public bool IsEnableAccount { get; set; } = true;
    //}

    public class UserEditDto : BXJG.Utils.Application.Share.User.UserEditDto
    {
        //public BXJG.Utils.Application.Share.User.UserEditDto EditDto { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(ZLJ.Core.Share.ZLJConsts.RemarkMaxLength)]
        [Display(Name = "备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 是否关联登录
        /// </summary>
        [Display(Name = "是否关联登录")]
        public bool IsEnableAccount { get; set; } = true;
    }
}
