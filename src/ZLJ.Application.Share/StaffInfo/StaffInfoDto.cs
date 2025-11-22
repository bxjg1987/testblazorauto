using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.OU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.User;

namespace ZLJ.Application.Share.StaffInfo
{
    /// <summary>
    /// 后台管理员工时的显示模型
    /// </summary>
    public class StaffInfoDto : StaffInfoCreateDto, IExtendableObj //,*///  UserDto //FullAuditedEntityDto<long>
    { 
        public new UserDto BaseDto { get; set; }=new UserDto();
        //public dynamic ExtensionData { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public string GenderText => Gender.GetDescription();
        /// <summary>
        /// 所属区域名
        /// </summary>
        [Display(Name = "所属区域")]
        public string? AreaDisplayName { get; set; }
        ///// <summary>
        ///// 所属公司和部门
        ///// </summary>
        //[Display(Name = "所属公司")]
        //public List<OUSelectDto>? Ous { get; set; }
        ///// <summary>
        ///// 所属公司和部门
        ///// </summary>
        //[Display(Name = "所属公司")]
        //public string OusText => Ous != default ? string.Join(',', Ous.Select(c => c?.Text)) : "";
        ///// <summary>
        ///// 所属公司Id
        ///// </summary>
        //[Display(Name = "所属公司")]
        //public IEnumerable<long> OuIds => Ous?.Select(c => c.Id);
        ///// <summary>
        ///// 所属岗位
        ///// </summary>
        //[Display(Name = "所属岗位")]
        //public List<PostForSelectDto> Posts { get; set; }
        ///// <summary>
        ///// 所属岗位
        ///// </summary>
        //[Display(Name = "所属岗位")]
        //public string PostsText => Posts != default ? string.Join(',', Posts.Select(c => c.DisplayName)) : string.Empty;
        ///// <summary>
        ///// 所属岗位
        ///// </summary>
        //[Display(Name = "所属岗位")]
        //public IEnumerable<string> PostNames => Posts?.Select(c => c.Name);

        public dynamic ExtensionData { get => ((IExtendableObj)BaseDto).ExtensionData; set => ((IExtendableObj)BaseDto).ExtensionData = value; }
    }
}