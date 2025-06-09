using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Share.Roles
{
    /// <summary>
    /// 后台管理角色使用的显示模型
    /// </summary>
    public class RoleDto : EntityDto<int>,IExtendableObj
    {
        public dynamic ExtensionData { get; set; }
        public IEnumerable<int> OuIds => Ous != null && Ous.Count > 0 ?  Ous.Select(c =>int.Parse( c.Id.ToString())) : new int[0];
        public string OusText => Ous != null && Ous.Count > 0 ? string.Join(',', Ous.Select(c => c.Text)) : "";
        [DisplayName("所属部门")]
        public List<OuDto> Ous { get; set; }
        //[Required]
        //[StringLength(AbpRoleBase.MaxNameLength)]
        [DisplayName("唯一名称")]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpRoleBase.MaxDisplayNameLength)]
        [DisplayName("显示名称")]
        public string DisplayName { get; set; }

        //public string NormalizedName { get; set; }

        // [StringLength(Role.MaxDescriptionLength)]
        [DisplayName("备注")]
        public string Description { get; set; }
        [DisplayName("系统预设")]
        public bool IsStatic { get; set; }
        [DisplayName("拥有的权限列表")]
        public string[]? GrantedPermissions { get; set; } = Array.Empty<string>();
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