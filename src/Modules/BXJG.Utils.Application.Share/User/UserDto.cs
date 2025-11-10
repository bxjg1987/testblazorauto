using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
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
    /// </summary>
    public class UserDto : UserSelectDto, IUserDto, IExtendableObj
    {
        /// <summary>
        /// 登陆吗
        /// </summary>
        [Display(Name = "登录名")]
        public string UserName { get; set; }
        ///// <summary>
        ///// 名称
        ///// </summary>
        // [Display(Name = "名称")]
        //public string Name { get; set; }
        ///// <summary>
        ///// 姓
        ///// </summary>
        // [Display(Name = "姓氏")]
        //public string Surname { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsActive { get; set; }
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

        //public string[] RoleNames { get; set; }

        public dynamic ExtensionData { get; set; }
    }
}
