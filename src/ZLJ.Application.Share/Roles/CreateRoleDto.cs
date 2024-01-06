using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ZLJ.Application.Share.Roles
{
    public class CreateRoleDto: RoleEditDto
    {
        ///// <summary>
        ///// 所属组织单位集合
        ///// </summary>
        //public long[] OuIds { get; set; }
        [DisplayName("唯一名称")]
        //[Required]
        //[StringLength(AbpRoleBase.MaxNameLength)]
        public string? Name { get; set; }

        //[Required]
        //[StringLength(AbpRoleBase.MaxDisplayNameLength)]
        //public string DisplayName { get; set; }

        //public string NormalizedName { get; set; }

        //[StringLength(Role.MaxDescriptionLength)]
        //public string Description { get; set; }

        //public List<string> GrantedPermissions { get; set; }
    }
}
