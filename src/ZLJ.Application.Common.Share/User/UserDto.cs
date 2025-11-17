using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.User
{
    public class UserDto :  BXJG.Utils.Application.Share.User.UserDto
    {
        //public  BXJG.Utils.Application.Share.User.UserDto BaseDto { get; set; }


        ///// <summary>
        ///// 是否关联登录
        ///// </summary>
        //[Display(Name = "是否关联登录")]
        //public bool IsEnableAccount { get; set; } = true;
        ///// <summary>
        ///// 备注
        ///// </summary>
        ////[StringLength(ZLJ.Core.Share.ZLJConsts.RemarkMaxLength)]
        //[Display(Name = "备注")]
        //public string? Remark { get; set; }
        //public Gender Gender { get; set; }
        ///// <summary>
        ///// 性别
        ///// </summary>
        //[Display(Name = "性别")]
        //public string GenderText => Gender.GetDescription();
    }
}
