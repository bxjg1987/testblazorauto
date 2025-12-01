using Abp.Application.Services.Dto;
using Abp.Auditing;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.OU;
using BXJG.Utils.Application.Share.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 用户后台管理端的用户查询模型
    /// 之定义 修改模型 不包含 的
    /// </summary>
    public class UserDto : /*UserSelectDto,*/IExtendableObj
    {
        /// <summary>
        /// 登陆名
        /// </summary>
        [StringLength(64)]
        [Display(Name = "登陆名")]
        public string? UserName { get; set; }
        ///// <summary>
        ///// 是否启用
        ///// </summary>
        //[Display(Name = "是否启用")]
        //public bool IsActive { get; set; }
        ///// <summary>
        ///// 多次登录失败锁定
        ///// </summary>
        //[Display(Name = "登录锁定")]
        //public bool IsLockoutEnabled { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Display(Name = "最后登录时间")]
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public virtual IEnumerable<RoleSelectDto>? Roles { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public virtual string? RolesText => Roles == null ? string.Empty : string.Join(',', Roles.Select(x => x.DisplayText));
        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        public virtual IEnumerable<IGeneralTree> Ous { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        public virtual string? OusText => Ous == null ? string.Empty : string.Join(',', Ous.Select(x => x.DisplayName));



        public dynamic ExtensionData { get; set; }
    }
}
