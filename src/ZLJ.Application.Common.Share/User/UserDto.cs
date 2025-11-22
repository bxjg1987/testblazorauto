using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Roles;
using BXJG.Utils.Application.Share.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Common.Share.User
{
    public class UserDto :  BXJG.Utils.Application.Share.User.UserDto
    {
        //public  BXJG.Utils.Application.Share.User.UserDto BaseDto { get; set; }
        //public new IEnumerable<OUSelectDto> Ous { get; set; }
        [JsonConverter(typeof(OusConverter<OUSelectDto>))]
        public override IEnumerable<IGeneralTree> Ous { get => base.Ous; set => base.Ous = value; }
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
