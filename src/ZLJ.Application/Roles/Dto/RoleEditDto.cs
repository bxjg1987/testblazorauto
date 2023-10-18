using System.Collections.Generic;
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
        public long[] OuIds { get; set; }
        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public List<string> GrantedPermissions { get; set; }
        //public bool IsStatic { get; set; }
    }
}