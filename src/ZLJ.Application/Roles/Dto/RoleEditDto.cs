using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using ZLJ.Authorization.Roles;

namespace ZLJ.App.Admin.Roles.Dto
{
    public class RoleEditDto: EntityDto<int>
    {
        //[Required]
        //[StringLength(AbpRoleBase.MaxNameLength)]
        //public string Name { get; set; }
        /// <summary>
        /// 所属组织单位集合
        /// </summary>
        [DisplayName("所属部门")]
        public long[] OuIds { get; set; }
        [DisplayName("显示名")]
        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
        [DisplayName("备注")]
        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }
        [DisplayName("拥有的权限")]
        public List<string> GrantedPermissions { get; set; }
        //public bool IsStatic { get; set; }
    }
}