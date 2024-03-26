using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using ZLJ.Application.Admin.Roles.Dto;
using BXJG.Utils.Localization;
using System.Linq;
using ZLJ.Application.Common.OU;
using ZLJ.Application.Common.Post;
using ZLJ.Application.Common.Users;
using ZLJ.Application.Common.Share.OU;
using BXJG.Common.Contracts;

namespace ZLJ.Application.Admin.BaseInfo.StaffInfo
{
    /// <summary>
    /// 后台管理员工时的显示模型
    /// </summary>
    public class StaffInfoDto : UserDto //FullAuditedEntityDto<long>
    {
        #region 基本信息
        //public long? PostId { get; set; }
        ///// <summary>
        ///// 名称
        ///// </summary>
        //public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string GenderText => this.Gender.ToLocalizationString();
        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }
        /// <summary>
        /// 生日yyyy-MM-dd
        /// </summary>
        public string BirthdayText => Birthday?.ToString("yyyy-MM-dd");

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTimeOffset? InDate { get; set; }
        public string InDateText => InDate?.ToString("yyyy-MM-dd");
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTimeOffset? OutDate { get; set; }
        public string OutDateText => OutDate?.ToString("yyyy-MM-dd");
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域名
        /// </summary>
        public string AreaDisplayName { get; set; }
        ///// <summary>
        ///// 关联账号的手机号
        ///// </summary>
        //public string PhoneNumber { get; set; }
        /// <summary>
        /// 现住地址
        /// </summary>
        public string CurrentAddress { get; set; }
        #endregion
        /// <summary>
        /// 所属公司和部门
        /// </summary>
        public List<OuDto> Ous { get; set; }
        /// <summary>
        /// 所属公司和部门
        /// </summary>
        public string OusText =>  Ous!=default? string.Join(',', Ous.Select(c=>c?.Text)):"";

        public IEnumerable<long> OuIds => Ous?.Select(c =>  c.Id);
        /// <summary>
        /// 所属岗位
        /// </summary>
        public List<PostDto> Posts { get; set; }
        /// <summary>
        /// 所属岗位
        /// </summary>
        public string PostsText => Posts != default ? string.Join(',', Posts.Select(c => c.DisplayName)) : "";
        public IEnumerable<string> PostNames => Posts?.Select(c => c.Name);
    }
}