using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Roles
{
    /// <summary>
    /// 抽象的角色编辑模型
    /// </summary>
    public class RoleEditDto : EntityDto<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(64)]
        [Display(Name = "名称")]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// 是否预设
        /// </summary>
        [Display(Name = "是否预设")]
        public virtual bool IsStatic { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        public virtual bool IsDefault { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        [Display(Name = "权限")]
        public string[]? GrantedPermissions { get; set; } = [];
        //[Display(Name = "权限")]
        //public List<string> PermissionNames { get; set; }

        public long[] OuIds { get; set; } = [];
    }
}
