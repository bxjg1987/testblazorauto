using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using ZLJ.App.Common.OU;
using ZLJ.Authorization.Roles;

namespace ZLJ.App.Admin.Roles.Dto
{
    /// <summary>
    /// 后台管理角色使用的显示模型
    /// </summary>
    public class RoleDto : EntityDto<int>
    {
        public IEnumerable<int> OuIds => Ous != null && Ous.Count > 0 ?  Ous.Select(c =>int.Parse( c.Id)) : new int[0];
        public string OusText => Ous != null && Ous.Count > 0 ? string.Join(',', Ous.Select(c => c.Text)) : "";
        public List<OuDto> Ous { get; set; }
        //[Required]
        //[StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        //public string NormalizedName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsStatic { get; set; }

        public List<string> GrantedPermissions { get; set; }
    }

    /// <summary>
    /// 不同类型用户关联到用户及其角色时的角色信息
    /// </summary>
    public class RoleRelationDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 角色备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否是静态角色
        /// </summary>
        public bool IsStatic { get; set; }
    }
}